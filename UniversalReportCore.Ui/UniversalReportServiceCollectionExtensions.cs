using Microsoft.Extensions.DependencyInjection;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;

namespace UniversalReportCore.Ui
{
    /// <summary>
    /// Provides extension methods for registering Universal Report services in an application's dependency injection container.
    /// </summary>
    public static class UniversalReportServiceCollectionExtensions
    {
        /// <summary>
        /// Registers core Universal Report services that do not require a database context.
        /// </summary>
        /// <param name="services">The dependency injection service collection.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddUniversalReport(this IServiceCollection services)
        {
            // Register core services (shared across all reports)
            services.AddScoped<IPageMetaFactory, PageMetaFactory>();
            services.AddScoped<IReportColumnFactory, ReportColumnFactory>();

            return services;
        }

        /// <summary>
        /// Registers all required services for a specific entity type used in Universal Reports.
        /// This method centralizes the registration of services related to an entity's report, 
        /// including query factories, metadata providers, column providers, page helpers, and filters.
        /// </summary>
        /// <typeparam name="T">The entity type for the report (e.g., ProductInventory).</typeparam>
        /// <typeparam name="TViewModel">The ViewModel type associated with the entity (e.g., ProductInventoryViewModel).</typeparam>
        /// <typeparam name="TQueryFactory">The query factory responsible for creating queries for the entity.</typeparam>
        /// <typeparam name="TPageMetaProvider">The provider responsible for page metadata related to the entity.</typeparam>
        /// <typeparam name="TReportColumnProvider">The provider responsible for defining report columns for the entity.</typeparam>
        /// <typeparam name="TQueryProvider">The provider responsible for executing paged queries for the entity.</typeparam>
        /// <typeparam name="TPageHelper">The helper responsible for handling report pages for the entity.</typeparam>
        /// <typeparam name="TFilterProvider">The filter provider responsible for handling filters for the entity.</typeparam>
        /// <param name="services">The dependency injection service collection.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
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
            // Register dependencies for the entity's report
            services.AddScoped<IQueryFactory<T>, TQueryFactory>(); // Handles query creation
            services.AddScoped<IPageMetaProvider, TPageMetaProvider>(); // Manages metadata for UI pages
            services.AddScoped<IReportColumnProvider, TReportColumnProvider>(); // Defines report columns
            services.AddScoped<IPagedQueryProvider<T>, TQueryProvider>(); // Provides paged query execution
            services.AddTransient<IReportPageHelper<T, TViewModel>, TPageHelper>(); // Handles report page operations
            services.AddSingleton<IFilterProvider<T>, TFilterProvider>(); // Provides filtering capabilities
            services.AddTransient<FilterFactory<T>>(); // Builds filter expressions for querying

            return services;
        }
    }
}
