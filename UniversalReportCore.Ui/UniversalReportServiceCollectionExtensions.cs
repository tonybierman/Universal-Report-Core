using Microsoft.Extensions.DependencyInjection;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;

namespace UniversalReportCore.Ui
{
    public static class UniversalReportServiceCollectionExtensions
    {
        public static IServiceCollection AddUniversalReport(this IServiceCollection services)
        {
            // Register core services (no DbContext dependency)
            services.AddScoped<IPageMetaFactory, PageMetaFactory>();
            services.AddScoped<IReportColumnFactory, ReportColumnFactory>();

            return services;
        }

        public static IServiceCollection AddEntityReportServices<
            T, TViewModel, TQueryFactory, TPageMetaProvider, TReportColumnProvider, TQueryProvider, TPageHelper, TFilterProvider>(
            this IServiceCollection services)
            where T : class
            where TViewModel : class
            where TQueryFactory : class, IQueryFactory<T>
            where TPageMetaProvider : class, IPageMetaProvider
            where TReportColumnProvider : class, IReportColumnProvider
            where TQueryProvider : class, IPagedQueryProvider<T>
            where TPageHelper : class, IReportPageHelper<T, TViewModel>
            where TFilterProvider : class, IFilterProvider<T>
        {
            services.AddScoped<IQueryFactory<T>, TQueryFactory>();
            services.AddScoped<IPageMetaProvider, TPageMetaProvider>();
            services.AddScoped<IReportColumnProvider, TReportColumnProvider>();
            services.AddScoped<IPagedQueryProvider<T>, TQueryProvider>();
            services.AddTransient<IReportPageHelper<T, TViewModel>, TPageHelper>();
            services.AddSingleton<IFilterProvider<T>, TFilterProvider>();
            services.AddTransient<FilterFactory<T>>();

            return services;
        }


    }
}
