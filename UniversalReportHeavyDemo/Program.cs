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
        await SeedCityPopulationDatabase(context, env, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

static bool EnsureDatabaseConnection(ApplicationDbContext context, ILogger logger)
{
    try
    {
        using var connection = context.Database.GetDbConnection();
        connection.Open();
        logger.LogInformation("Database connection established successfully.");
        return true;
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to establish a database connection.");
        return false;
    }
}

/// <summary>
/// Seeds the database with data from a CSV file.
/// </summary>
static async Task SeedCityPopulationDatabase(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
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
    
    bool tableExists = context.Database.ExecuteSqlRaw(@"
        SELECT COUNT(*) FROM information_schema.tables 
        WHERE table_schema = DATABASE() 
        AND table_name = 'CityPopulations';") > 0;

    if (!tableExists)
    {
        RelationalDatabaseCreator databaseCreator =
            (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
        await databaseCreator.CreateTablesAsync();
        await context.SaveChangesAsync();
        EnsureIndexesCreated(context);
    }

    // Seed if empty
    if (!context.CityPopulations.Any())
    {
        await context.CityPopulations.AddRangeAsync(rawRecords);
        await context.SaveChangesAsync();
        logger.LogInformation("Database seeded successfully.");
    }
    else
    {
        logger.LogInformation("CityPopulation table already contains data. Skipping seed.");
    }

    if (!context.CityPopulationCohorts.Any())
    {
        CityPopulationCohort male = new CityPopulationCohort()
        {
            Id = 1,
            Name = "Male"
        };

        CityPopulationCohort female = new CityPopulationCohort()
        {
            Id = 2,
            Name = "Female"
        };
        await context.CityPopulationCohorts.AddAsync(male);
        await context.CityPopulationCohorts.AddAsync(female);
        await context.SaveChangesAsync();

        var maleCohort = await context.CityPopulationCohorts
            .Include(c => c.CityPopulations)
            .FirstAsync(a => a.Id == 1);

        await context.CityPopulations
            .Where(a => a.Sex == "Male")
            .ForEachAsync(male => maleCohort.CityPopulations.Add(male));

        await context.SaveChangesAsync();

        var femaleCohort = await context.CityPopulationCohorts
            .Include(c => c.CityPopulations)
            .FirstAsync(a => a.Id == 2);

        await context.CityPopulations
            .Where(a => a.Sex == "Female")
            .ForEachAsync(female => femaleCohort.CityPopulations.Add(female));

        await context.SaveChangesAsync();
    }
}

static void EnsureIndexesCreated(ApplicationDbContext context)
{
    string indexSql = @"
        CREATE INDEX IF NOT EXISTS idx_city_sex_year 
        ON CityPopulations (City(100), Sex(10), Year DESC);";

    context.Database.ExecuteSqlRaw(indexSql);
}

