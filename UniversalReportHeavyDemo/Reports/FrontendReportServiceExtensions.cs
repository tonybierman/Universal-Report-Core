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

        public static IServiceCollection AddFrontendReportServices(this IServiceCollection services)
        {
            services.AddScoped<IReportPageHelperFactory, HeavyDemoPageHelperFactory>();

            // Register IUniversalReportService on the frontend with ApplicationDbContext
            services.AddScoped<IUniversalReportService>(provider =>
            {
                var dbContext = provider.GetRequiredService<ApplicationDbContext>();
                var mapper = provider.GetRequiredService<IMapper>();
                return new UniversalReportService(dbContext, mapper);
            });

            services.AddCityPopulationServices();
            services.AddNationalGdpServices();

            return services;
        }
    }
}
