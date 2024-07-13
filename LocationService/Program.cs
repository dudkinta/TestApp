/*
 * This service need to get countries and provinces
 */
using CommonLibrary;
using LocationContextDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Setting Serilog for save logs to file/ In prod need change appsettings.json for sending logs to Elastic
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var privateServiceKey = builder.Configuration.GetSection("AppSettings:PrivateServiceKey").Value ?? string.Empty;

// Add context
builder.Services.AddDbContext<ILocationContext, LocationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<BadRequestFilter>();
});

#if (DEBUG)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5000", "http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
#endif

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Service", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(privateServiceKey))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ServicePolicy", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "service"));
});

var app = builder.Build();

#if (DEBUG) // Check DB. Do not use in prod
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LocationContext>();
    context.Database.EnsureCreated();
    DataInitializer.Initialize(context);
}
#endif

app.MapControllers();

#if (DEBUG)
app.UseCors("AllowSpecificOrigin");
#endif

app.Run();

