using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using UniversalReportDemo.Data;
using UniversalReportDemo.Import;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.PagedQueries;
using UniversalReportCore;
using UniversalReportDemo.Reports;
using UniversalReportDemo.Reports.CityPop;
using AutoMapper;
using UniversalReport.Services;
using ProductionPlanner.Maps;
using UniversalReportDemo.ViewModels;
using UniversalReportDemo.Reports.CountryGdp;

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

// Add In-Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

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

// Seed the database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    SeedCityPopulationDatabase(dbContext, env, logger);
    SeedNationalGdpDatabase(dbContext, env, logger);
}

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

app.Run();

/// <summary>
/// Seeds the database with data from a CSV file.
/// </summary>
static void SeedCityPopulationDatabase(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
{
    string csvFilePath = Path.Combine(env.WebRootPath, "data", "city_populations.csv");

    if (!System.IO.File.Exists(csvFilePath))
    {
        logger.LogError($"CSV file not found: {csvFilePath}");
        return;
    }

    using var stream = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    using var reader = new StreamReader(stream);
    using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null,
        MissingFieldFound = null,
    });

    csv.Context.RegisterClassMap<CityPopulationsRecordMap>();

    var rawRecords = csv.GetRecords<CityPopulation>().ToList();
    logger.LogInformation($"Loaded {rawRecords.Count} records from CSV.");

    if (!context.CityPopulations.Any()) // Prevent duplicate seed
    {
        int idCounter = 1; // Start at 1, or use the highest existing ID

        foreach (var record in rawRecords)
        {
            record.Id = idCounter++; // Assign unique IDs
        }

        context.CityPopulations.AddRange(rawRecords);






        context.SaveChanges();
    }
}

static void SeedNationalGdpDatabase(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
{
    string csvFilePath = Path.Combine(env.WebRootPath, "data", "national_gdp.csv");

    if (!System.IO.File.Exists(csvFilePath))
    {
        logger.LogError($"CSV file not found: {csvFilePath}");
        return;
    }

    using var stream = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    using var reader = new StreamReader(stream);
    using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null,
        MissingFieldFound = null,
    });

    csv.Context.RegisterClassMap<NationalGdpRecordMap>();

    var rawRecords = csv.GetRecords<NationalGdp>().ToList();
    logger.LogInformation($"Loaded {rawRecords.Count} records from CSV.");

    if (!context.NationalGdps.Any()) // Prevent duplicate seed
    {
        int idCounter = 1; // Start at 1, or use the highest existing ID

        foreach (var record in rawRecords)
        {
            record.Id = idCounter++; // Assign unique IDs
        }

        context.NationalGdps.AddRange(rawRecords);
        context.SaveChanges();
    }
}
