using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Xunit;
using System;
using ProductionPlanner.Maps;
using UniversalReport.Services;
using UniversalReportDemo.Data;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportDemo.Reports.CityPop;
using UniversalReportDemo.Reports;
using UniversalReportDemo.ViewModels;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using UniversalReportDemo.Import;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace UniversalReportCore.Tests
{


    public class TestStartupFixture : IDisposable
    {
        private readonly string _csvFileName = "city_populations.csv";

        public ServiceProvider ServiceProvider { get; private set; }

        public TestStartupFixture()
        {


            var services = new ServiceCollection();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CityPopulationMappingProfile());
                mc.AddProfile(new NationalGdpMappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Add In-Memory Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));

            // *** Universal Reports
            services.AddScoped<IUniversalReportService>(provider =>
            {
                var dbContext = provider.GetRequiredService<ApplicationDbContext>();
                var mapper = provider.GetRequiredService<IMapper>();
                return new UniversalReportService(dbContext, mapper);
            });

            // Universal Reports
            services.AddScoped<IPageMetaFactory, PageMetaFactory>();
            services.AddTransient<IPageMetaFactory, PageMetaFactory>();
            services.AddScoped<IReportColumnFactory, ReportColumnFactory>();
            services.AddScoped<PageHelperFactory>();

            // ** CityPopulation
            // Entity Type
            services.AddScoped<IQueryFactory<CityPopulation>, CityPopulationQueryFactory>();
            // Report Instances
            services.AddScoped<IPageMetaProvider, CityPopulationDemoPageMetaProvider>();
            services.AddScoped<IReportColumnProvider, CityPopulationDemoReportColumnProvider>();
            services.AddScoped<IPagedQueryProvider<CityPopulation>, CityPopulationDemoQueryProvider>();
            services.AddTransient(typeof(IPageHelper<CityPopulation, CityPopulationViewModel>), typeof(CityPopulationDemoPageHelper));

            // Build the ServiceProvider
            ServiceProvider = services.BuildServiceProvider();

            // Ensure the database is created and seeded
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TestStartupFixture>>();
            SeedCityPopulationDatabase(context, _csvFileName, logger);
        }

        static void SeedCityPopulationDatabase(ApplicationDbContext context, string csvFname, ILogger logger)
        {
            string csvFilePath = Path.Combine("Data", "csvFname");

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

        public void Dispose()
        {
            // Dispose of the ServiceProvider
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
