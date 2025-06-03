using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Maps;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Reports;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CityPopulationMappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var hostName = Environment.GetEnvironmentVariable("HOSTNAME");

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add Universal Report services
builder.Services.AddUniversalReportServices();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var env = services.GetRequiredService<IWebHostEnvironment>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        // Delegate all database work to DataSeed
        await DataSeed.EnsureDatabaseAndSeedAsync(context, env, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while setting up the database.");
    }
}

app.Run();

