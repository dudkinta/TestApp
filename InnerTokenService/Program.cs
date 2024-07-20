using CommonLibrary;
using Consul;
using InnerTokenService.Helpers;
using InnerTokenService.Interfaces;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

AddFilters(builder.Services);

AddServices(builder.Services, builder.Configuration);

AddSwagger(builder.Services);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

EnableSwagger(app);

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

    var privateServiceKey = configuration.GetSection("AppSettings:PrivateServiceKey").Value ?? string.Empty;
    services.AddScoped<ITokenHelper, JwtTokenHelper>((a) => new JwtTokenHelper(privateServiceKey));

    var consulServiceConfig = new ConsulServiceConfiguration
    {
        Name = "InnerTokenService",
        Address = "localhost",
        Port = 5002,
        HealthEndpoint = "api/health/"
    };

    services.AddSingleton(consulServiceConfig);

    //Consul client
    var consulEndpoint = configuration.GetSection("AppSettings:ConsulEndpoint").Value ?? string.Empty;
    services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
    {
        cfg.Address = new Uri(consulEndpoint);
    }));
    services.AddHostedService<ConsulHostedService>();
}

void AddSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Inner Token API",
            Description = "A simple example ASP.NET Core Web API for generate inner API token",
        });
    });
}

void EnableSwagger(WebApplication? app)
{
    if (app == null)
        return;

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inner Token API V1");
            c.RoutePrefix = string.Empty;
        });
    }
}