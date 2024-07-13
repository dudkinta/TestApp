/*
 * This service need to save new users
 */
using CommonLibrary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RegistrationService.Services;
using Serilog;
using UserContextDb;

var builder = WebApplication.CreateBuilder(args);

// Setting Serilog for save logs to file/ In prod need change appsettings.json for sending logs to Elastic
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddScoped<IPasswordService, PasswordService>();

// Add context
builder.Services.AddDbContext<IUserContext, UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<BadRequestFilter>();
});

#if (DEBUG)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
#endif

var app = builder.Build();


#if (DEBUG) // Check DB. Do not use in prod
using (var scope = app.Services.CreateScope())  
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserContext>();
    context.Database.EnsureCreated();
}
#endif

#if (DEBUG) //Only for test Front App. Do not use in prod 
app.UseStaticFiles(new StaticFileOptions            
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});
app.MapFallbackToFile("index.html");
#endif

app.MapControllers();

#if (DEBUG)
app.UseCors("AllowSpecificOrigin");
#endif

app.Run();