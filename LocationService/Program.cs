/*
 * This service need to get countries and provinces
 */
using LocationContextDb;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setting Serilog for save logs to file/ In prod need change appsettings.json for sending logs to Elastic
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add context
builder.Services.AddDbContext<ILocationContext, LocationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

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
app.Run();
