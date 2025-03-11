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
    }
}
