using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using UniversalReportDemo.Data;
using UniversalReportDemo.Import;

var builder = WebApplication.CreateBuilder(args);

// Add In-Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddRazorPages();

var app = builder.Build();

// Seed the database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    SeedDatabase(dbContext, env, logger);
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
static void SeedDatabase(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
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

