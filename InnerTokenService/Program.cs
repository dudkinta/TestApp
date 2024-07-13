using CommonLibrary;
using InnerTokenService.Helpers;
using InnerTokenService.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setting Serilog for save logs to file/ In prod need change appsettings.json for sending logs to Elastic
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var privateServiceKey = builder.Configuration.GetSection("AppSettings:PrivateServiceKey").Value ?? string.Empty;
builder.Services.AddControllers(options =>
{
    options.Filters.Add<BadRequestFilter>();
});
builder.Services.AddScoped<ITokenHelper, JwtTokenHelper>((a) => new JwtTokenHelper(privateServiceKey));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();