using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using UniversalReportHeavyDemo.Import;
using Microsoft.Extensions.Logging;

namespace UniversalReportHeavyDemo.Data
{
    public class DataSeed
    {
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
        public static async Task SeedCityPopulationDatabase(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
        {
            logger.LogInformation("Determining if CityPopulations table exists.");

            bool tableExists = context.Database.ExecuteSqlRaw(@"
                SELECT COUNT(*) FROM information_schema.tables 
                WHERE table_schema = DATABASE() 
                AND table_name = 'CityPopulations';") > 0;

            if (!tableExists)
            {
                logger.LogInformation("CityPopulations table not found. Creating...");

                RelationalDatabaseCreator databaseCreator =
                    (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                await databaseCreator.CreateTablesAsync();
                await context.SaveChangesAsync();

                EnsureIndexesCreated(context, logger);
            }

            // Seed if empty
            if (!context.CityPopulations.Any())
            {
                logger.LogInformation("Seeding CityPopulations table from CSV file.");

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
                logger.LogInformation("Creating CityPopulationCohorts.");

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

                logger.LogInformation("Seeding CityPopulationCohorts.");

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

                logger.LogInformation("CityPopulation seeding completed.");
            }
        }

        static void EnsureIndexesCreated(ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Creating CityPopulations prefix index.");

            string indexSql = @"
        CREATE INDEX IF NOT EXISTS idx_city_sex_year 
        ON CityPopulations (City(100), Sex(10), Year DESC);";

            context.Database.ExecuteSqlRaw(indexSql);
        }
    }
}
