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
        /// <summary>
        /// Ensures the database is ready (migrations applied, tables created) and seeds it.
        /// </summary>
        public static async Task EnsureDatabaseAndSeedAsync(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
        {
            logger.LogInformation("Ensuring database is ready and seeded...");

            // Step 1: Apply migrations
            await EnsureDatabaseMigratedAsync(context, logger);

            // Step 2: Seed the database
            await SeedCityPopulationDatabase(context, env, logger);
        }

        private static async Task EnsureDatabaseMigratedAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("Applying database migrations...");
                await context.Database.MigrateAsync(); // Applies migrations to create the database schema
                await context.SaveChangesAsync();
                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to apply database migrations.");
                throw; // Re-throw to let the caller handle the failure
            }
        }

        /// <summary>
        /// Seeds the database with city population data and cohorts.
        /// </summary>
        public static async Task SeedCityPopulationDatabase(ApplicationDbContext context, IWebHostEnvironment env, ILogger logger)
        {
            // Since migrations are already applied, we assume tables exist or will be handled by EF
            logger.LogInformation("Checking CityPopulations table...");

            // Seed CityPopulations if empty
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
                logger.LogInformation("CityPopulations table seeded successfully.");
            }
            else
            {
                logger.LogInformation("CityPopulation table already contains data. Skipping seed.");
            }

            // Seed CityPopulationCohorts if empty
            if (!context.CityPopulationCohorts.Any())
            {
                logger.LogInformation("Creating CityPopulationCohorts.");

                CityPopulationCohort male = new CityPopulationCohort { Id = 1, Name = "Male" };
                CityPopulationCohort female = new CityPopulationCohort { Id = 2, Name = "Female" };

                await context.CityPopulationCohorts.AddAsync(male);
                await context.CityPopulationCohorts.AddAsync(female);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeding CityPopulationCohorts with mappings.");

                var maleCohort = await context.CityPopulationCohorts
                    .Include(c => c.CityPopulations)
                    .FirstAsync(a => a.Id == 1);

                await context.CityPopulations
                    .Where(a => a.Sex == "Male")
                    .ForEachAsync(male => maleCohort.CityPopulations.Add(male));

                var femaleCohort = await context.CityPopulationCohorts
                    .Include(c => c.CityPopulations)
                    .FirstAsync(a => a.Id == 2);

                await context.CityPopulations
                    .Where(a => a.Sex == "Female")
                    .ForEachAsync(female => femaleCohort.CityPopulations.Add(female));

                await context.SaveChangesAsync();
                logger.LogInformation("CityPopulationCohorts seeding completed.");
            }

            // Ensure indexes are created
            EnsureIndexesCreated(context, logger);
        }

        private static void EnsureIndexesCreated(ApplicationDbContext context, ILogger logger)
        {
            logger.LogInformation("Ensuring indexes are created...");

            string indexSql = @"CREATE INDEX IF NOT EXISTS idx_city_sex_year_id ON CityPopulations (City(100), Sex(10), Year DESC, Id);";
            context.Database.ExecuteSqlRaw(indexSql);

            indexSql = @"CREATE INDEX IF NOT EXISTS idx_city_population_cohort ON CityPopulationCohorts(Id);";
            context.Database.ExecuteSqlRaw(indexSql);

            indexSql = @"CREATE INDEX IF NOT EXISTS idx_city_population_cohort_mapping ON CityPopulationCohortMapping(CityPopulationId, CohortId);";
            context.Database.ExecuteSqlRaw(indexSql);

            logger.LogInformation("Indexes ensured successfully.");
        }
    }
}