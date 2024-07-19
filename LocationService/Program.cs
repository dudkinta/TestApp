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

AddServices(builder.Services, builder.Configuration);

AddFilters(builder.Services);

AddCors(builder.Services);

AddJWTAuthorize(builder.Services, builder.Configuration);

var app = builder.Build();

CheckDb(app);

app.MapControllers();

EnableCors(app);

app.Run();


void AddFilters(IServiceCollection services)
{

    services.AddControllers(options =>
    {
        options.Filters.Add<BadRequestFilter>();
    });
}

void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // Setting Serilog for save logs to file/ In prod need change appsettings.json for sending logs to Elastic
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    // Add context
    builder.Services.AddDbContext<ILocationContext, LocationContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

void AddJWTAuthorize(IServiceCollection services, IConfiguration configuration)
{
    var privateServiceKey = configuration.GetSection("AppSettings:PrivateServiceKey").Value ?? string.Empty;
    services.AddAuthentication(options =>
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
    services.AddAuthorization(options =>
    {
        options.AddPolicy("ServicePolicy", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "service"));
    });
}

void AddCors(IServiceCollection services)
{
#if (DEBUG)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:5000", "http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });
#endif
}

void EnableCors(WebApplication? app)
{
#if (DEBUG)
    app?.UseCors("AllowSpecificOrigin");
#endif
}

void CheckDb(WebApplication? app)
{
    if (app == null)
        return;

#if (DEBUG) // Check DB. Do not use in prod
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<LocationContext>();
        context.Database.EnsureCreated();
        DataInitializer.Initialize(context);
    }
#endif
}
