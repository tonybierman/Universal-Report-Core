using Microsoft.Extensions.DependencyInjection;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Reports.CityPop;
using UniversalReportHeavyDemo.ViewModels;
using UniversalReport.Services;
using AutoMapper;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;
using UniversalReportCore;
using UniversalReportHeavyDemo.Reports.Domain;

namespace UniversalReportHeavyDemo.Reports
{
    public static class ReportServiceExtensions
    {
        /// <summary>
        /// Registers CityPopDemo report.
        /// </summary>
        public static IServiceCollection AddCityPopDemoReport(this IServiceCollection services) =>
            services.AddEntityReportServices<
                CityPopulation,
                CityPopulationViewModel,
                CityPopulationQueryFactory,
                CityPopDemoPageMetaProvider,
                CityPopDemoReportColumnProvider,
                CityPopDemoQueryProvider,
                CityPopDemoPageHelper,
                CityPopulationFilterProvider>();

        public static IServiceCollection AddUniversalReportServices(this IServiceCollection services)
        {
            // Add Universal Report Core services
            services.AddUniversalReport();

            // Register the IUniversalReportService here
            services.AddScoped<IUniversalReportService>(provider =>
            {
                var dbContext = provider.GetRequiredService<ApplicationDbContext>();
                var mapper = provider.GetRequiredService<IMapper>();
                return new UniversalReportService(dbContext, mapper);
            });

            services.AddScoped<IReportPageHelperFactory, PageHelperFactory>();
            services.AddCityPopDemoReport();

            return services;
        }
    }
}
