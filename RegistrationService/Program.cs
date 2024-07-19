/*
 * This service need to save new users
 */
using ApiHelper;
using CommonLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RegistrationService.Services;
using Serilog;
using UserContextDb;

var builder = WebApplication.CreateBuilder(args);

AddServices(builder.Services, builder.Configuration);

AddFilters(builder.Services);

AddCors(builder.Services);

var app = builder.Build();

CheckDb(app);

AddFrontFiles(app);

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

    //add validators
    services.AddScoped<IPasswordService, PasswordService>();
    services.AddScoped<IEmailValidator, EmailValidator>();

    //add innerApiClient
    var innerTokenEndpoint = configuration.GetSection("AppSettings:InnerTokenEndpoint").Value ?? string.Empty;
    services.AddScoped<IInnerApiClient, InnerApiClient>(_ => { return new InnerApiClient(innerTokenEndpoint); });

    // Add context
    services.AddDbContext<IUserContext, UserContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

void AddCors(IServiceCollection services)
{
#if (DEBUG)
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:4200")
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

void AddFrontFiles(WebApplication? app)
{
#if (DEBUG) //Only for test Front App. Do not use in prod 
    app?.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
        RequestPath = ""
    });
    app.MapFallbackToFile("index.html");
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
        var context = services.GetRequiredService<UserContext>();
        context.Database.EnsureCreated();
    }
#endif
}