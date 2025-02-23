using AutoMapper;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Maps;
using System.Globalization;
using UniversalReport.Services;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Import;
using UniversalReportHeavyDemo.Reports.CityPop;
using UniversalReportHeavyDemo.Reports.CountryGdp;
using UniversalReportHeavyDemo.Reports;
using UniversalReportHeavyDemo.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CityPopulationMappingProfile());
    mc.AddProfile(new NationalGdpMappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// *** Universal Reports
builder.Services.AddScoped<IUniversalReportService>(provider =>
{
    var dbContext = provider.GetRequiredService<ApplicationDbContext>();
    var mapper = provider.GetRequiredService<IMapper>();
    return new UniversalReportService(dbContext, mapper);
});

// Universal Reports
builder.Services.AddScoped<IPageMetaFactory, PageMetaFactory>();
builder.Services.AddTransient<IPageMetaFactory, PageMetaFactory>();
builder.Services.AddScoped<IReportColumnFactory, ReportColumnFactory>();
builder.Services.AddScoped<PageHelperFactory>();

// ** CityPopulation
// Entity Type
builder.Services.AddScoped<IQueryFactory<CityPopulation>, CityPopulationQueryFactory>();
// Report Instances
builder.Services.AddScoped<IPageMetaProvider, CityPopulationDemoPageMetaProvider>();
builder.Services.AddScoped<IReportColumnProvider, CityPopulationDemoReportColumnProvider>();
builder.Services.AddScoped<IPagedQueryProvider<CityPopulation>, CityPopulationDemoQueryProvider>();
builder.Services.AddTransient(typeof(IReportPageHelper<CityPopulation, CityPopulationViewModel>), typeof(CityPopulationDemoPageHelper));

// ** NationalGdp
// Entity Type
builder.Services.AddScoped<IQueryFactory<NationalGdp>, NationalGdpQueryFactory>();
// Report Instances
builder.Services.AddScoped<IPageMetaProvider, CountryGdpDemoPageMetaProvider>();
builder.Services.AddScoped<IReportColumnProvider, CountryGdpDemoReportColumnProvider>();
builder.Services.AddTransient(typeof(IReportPageHelper<NationalGdp, NationalGdpViewModel>), typeof(CountryGdpDemoPageHelper));
builder.Services.AddScoped<IPagedQueryProvider<NationalGdp>, CountryGdpDemoQueryProvider>();

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

// Get the service provider
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var env = services.GetRequiredService<IWebHostEnvironment>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // **Apply Migrations Here (Outside of Seeding)**
        logger.LogInformation("Ensuring database...");
        await context.Database.MigrateAsync(); // Ensures tables exist
        await context.SaveChangesAsync();

        // **Now Seed Database**
        await DataSeed.SeedCityPopulationDatabase(context, env, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

