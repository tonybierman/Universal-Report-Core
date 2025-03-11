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
        public static IServiceCollection AddCityPopulationServices(this IServiceCollection services)
        {
            services.AddScoped<IQueryFactory<CityPopulation>, CityPopulationQueryFactory>();
            services.AddScoped<IPageMetaProvider, CityPopulationDemoPageMetaProvider>();
            services.AddScoped<IReportColumnProvider, CityPopulationDemoReportColumnProvider>();
            services.AddScoped<IPagedQueryProvider<CityPopulation>, CityPopulationDemoQueryProvider>();
            services.AddTransient<IReportPageHelper<CityPopulation, CityPopulationViewModel>, CityPopulationDemoPageHelper>();
            services.AddSingleton<IFilterProvider<CityPopulation>, CityPopulationFilterProvider>();
            services.AddTransient<FilterFactory<CityPopulation>>();
            return services;
        }

        public static IServiceCollection AddNationalGdpServices(this IServiceCollection services)
        {
            services.AddScoped<IQueryFactory<NationalGdp>, NationalGdpQueryFactory>();
            services.AddScoped<IPageMetaProvider, CountryGdpDemoPageMetaProvider>();
            services.AddScoped<IReportColumnProvider, CountryGdpDemoReportColumnProvider>();
            services.AddScoped<IPagedQueryProvider<NationalGdp>, CountryGdpDemoQueryProvider>();
            services.AddTransient<IReportPageHelper<NationalGdp, NationalGdpViewModel>, CountryGdpDemoPageHelper>();
            services.AddSingleton<IFilterProvider<NationalGdp>, NationalGdpFilterProvider>();
            services.AddTransient<FilterFactory<NationalGdp>>();
            return services;
        }

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
