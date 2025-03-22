using Microsoft.Extensions.DependencyInjection;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Reports.CityPop;
using UniversalReportHeavyDemo.Reports.CountryGdp;
using UniversalReportHeavyDemo.ViewModels;
using UniversalReport.Services;
using AutoMapper;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;
using UniversalReportCore;

namespace UniversalReportHeavyDemo.Reports
{
    public static class FrontendReportServiceExtensions
    {
        /// <summary>
        /// Registers CityPopulation report services.
        /// </summary>
        public static IServiceCollection AddCityPopulationServices(this IServiceCollection services) =>
            services.AddEntityReportServices<
                CityPopulation,
                CityPopulationViewModel,
                CityPopulationQueryFactory,
                CityPopulationDemoPageMetaProvider,
                CityPopulationDemoReportColumnProvider,
                CityPopulationDemoQueryProvider,
                CityPopulationDemoPageHelper,
                CityPopulationFilterProvider>();

        /// <summary>
        /// Registers NationalGdp report services.
        /// </summary>
        public static IServiceCollection AddNationalGdpServices(this IServiceCollection services) =>
            services.AddEntityReportServices<
                NationalGdp,
                NationalGdpViewModel,
                NationalGdpQueryFactory,
                CountryGdpDemoPageMetaProvider,
                CountryGdpDemoReportColumnProvider,
                CountryGdpDemoQueryProvider,
                CountryGdpDemoPageHelper,
                NationalGdpFilterProvider>();

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

            services.AddScoped<IReportPageHelperFactory, HeavyDemoPageHelperFactory>();
            services.AddCityPopulationServices();
            services.AddNationalGdpServices();

            return services;
        }
    }
}
