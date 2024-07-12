/*
 * This service need to save new users
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using UserContextDb;

var builder = WebApplication.CreateBuilder(args);

// setting Serilog for save logs to file/ In prod need change appsettings.json for sending logs to Elastic
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

//add context
builder.Services.AddDbContext<IUserContext, UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// check DB. Do not use in prod
using (var scope = app.Services.CreateScope())  
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserContext>();
    context.Database.EnsureCreated();
}

#region Angular
//Do not use in prod
app.UseStaticFiles(new StaticFileOptions            
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});
app.UseRouting(); 

app.MapControllers();
app.MapFallbackToFile("index.html");
#endregion

app.Run();