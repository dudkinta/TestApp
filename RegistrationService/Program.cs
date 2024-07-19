/*
 * This service need to save new users
 */
using ApiHelper;
using CommonLibrary;
using Consul;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RegistrationService.Services;
using RegistrationService.Validators;
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

    // Add context
    services.AddDbContext<IUserContext, UserContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    //Add validators
    builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

    //Consul client
    var consulEndpoint = configuration.GetSection("AppSettings:ConsulEndpoint").Value ?? string.Empty;
    services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
    {
        cfg.Address = new Uri(consulEndpoint);
    }));
    services.AddHostedService<ConsulHostedService>();

    //Consul resolver
    services.AddTransient<ConsulServiceDiscovery>();

    //Add InnerApiClient
    services.AddScoped<IInnerApiClient, InnerApiClient>();
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