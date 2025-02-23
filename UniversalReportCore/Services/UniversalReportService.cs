using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using UniversalReportCore;
using UniversalReportCore.Helpers;
using UniversalReportCore.PagedQueries;

namespace UniversalReport.Services
{
    /// <summary>
    /// Service for generating paginated reports with support for dynamic filtering, sorting, and mapping entities to view models.
    /// Utilizes AutoMapper for entity-to-view model conversion and Entity Framework for query execution.
    /// </summary>
    public class UniversalReportService : IUniversalReportService
    {
        private readonly IMapper _mapper;
        private readonly DbContext _dbContext;

        public UniversalReportService(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of mapped view models based on the given query parameters.
        /// Supports filtering, cohort-based logic, additional custom filters, and sorting.
        /// </summary>
        /// <typeparam name="TEntity">The type of the database entity.</typeparam>
        /// <typeparam name="TViewModel">The type of the resulting view model.</typeparam>
        /// <param name="parameters">Query parameters for filtering, sorting, pagination, and custom logic.</param>
        /// <param name="query">An optional initial query. If null, it defaults to querying all records of <typeparamref name="TEntity"/>.</param>
        /// <returns>A paginated list of <typeparamref name="TViewModel"/> with aggregates and metadata.</returns>
        public async Task<PaginatedList<TViewModel>> GetPagedAsync<TEntity, TViewModel>(
            PagedQueryParameters<TEntity> parameters,
            IQueryable<TEntity>? query = null
        ) where TEntity : class where TViewModel : class
        {
            var stopwatch = Stopwatch.StartNew();

            // If no query is provided, initialize it with the DbSet for TEntity
            query = query ?? _dbContext.Set<TEntity>();

            // SKU Filter: Apply a filter if the "Sku" property exists on the entity and a SKU value is specified in the parameters
            //if (!string.IsNullOrEmpty(parameters.Sku) && typeof(TEntity).GetProperty("Sku") != null)
            //{
            //    query = query.Where(e => EF.Property<string>(e, "Sku") == parameters.Sku);
            //}

            // Apply Date Range filter
            if (parameters.DateFilter != null && parameters.DateFilter.HasValue)
            {
                query = ApplyDateRangeFilter(query, parameters.DateFilter);
            }

            // Cohort Filter: Apply cohort-based logic if CohortIds and CohortLogic are provided
            if (parameters.CohortIds != null && parameters.CohortIds.Any() && parameters.CohortLogic != null)
            {
                query = parameters.CohortLogic(query, parameters.CohortIds);
            }

            // Additional Filters: Apply custom filters provided in the AdditionalFilter delegate
            if (parameters.AdditionalFilter != null)
            {
                query = parameters.AdditionalFilter(query);
            }

            // Sorting: Apply sorting if a sort order is specified in the parameters
            if (!string.IsNullOrEmpty(parameters.Sort))
            {
                query = ApplySorting(query, parameters.Sort, parameters.ReportColumns.ToArray());
            }

            // Ensure that AggregateLogic is not null. If it is, initialize it to return an empty dictionary.
            parameters.AggregateLogic ??= async src => new Dictionary<string, dynamic>();

            // Return a paginated list with the following:
            // - The query executed with AsNoTracking for better performance
            // - Pagination using PageIndex and ItemsPerPage
            // - Entities mapped to TViewModel using AutoMapper
            // - Aggregate logic for summaries (e.g., sums, counts)
            // - Optional meta logic for additional metadata
            var retval =  await PaginatedList<TViewModel>.CreateMappedWithAggregatesAsync(
                query.AsNoTracking(),
                parameters.PageIndex ?? 1,
                parameters.ItemsPerPage ?? 25, // TODO: get rid of Magic number
                entity => _mapper.Map<TViewModel>(entity),  // Map each entity to a view model using AutoMapper
                parameters.AggregateLogic,
                parameters.MetaLogic
            );

            stopwatch.Stop();

            if (retval.Meta != null)
            {
                retval.Meta["QueryDuration"] = $"{stopwatch.Elapsed.TotalMilliseconds:N2} ms";
            }

            return retval;
        }


        /// <summary>
        /// Applies sorting to the given query based on the specified sort order and list of column definitions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities in the query.</typeparam>
        /// <param name="query">The query to apply sorting to.</param>
        /// <param name="sortOrder">The sort order string (e.g., "SkuDesc" or "SkuAsc").</param>
        /// <param name="columns">A list of column definitions to validate sortable columns.</param>
        /// <returns>The query with the sorting applied.</returns>
        private IQueryable<TEntity> ApplySorting<TEntity>(
            IQueryable<TEntity> query,
            string sortOrder,
            IReportColumnDefinition[] columns)
        {
            // TODO: Fix bracket removal hack
            sortOrder = sortOrder.Replace("{", "").Replace("}", "");

            // Determine if the sort order is descending by checking if the sortOrder ends with "Desc".
            var isDescending = SortHelper.IsDescending(sortOrder);

            // Extract the base sort key by removing "Desc" or "Asc" from the sort order string.
            var baseSortKey = SortHelper.BaseSortKey(sortOrder);

            // Find the corresponding column definition that matches the extracted sort key.
            var column = columns.FirstOrDefault(c => c.PropertyName == baseSortKey);

            // If a matching column is found and it is marked as sortable:
            if (column != null && column.IsSortable)
            {
                // Use System.Linq.Dynamic.Core to apply sorting dynamically based on the column property name.
                query = query.OrderBy($"{column.PropertyName} {(isDescending ? "descending" : "ascending")}");
            }

            // Return the sorted query.
            return query;
        }

        private IQueryable<TEntity> ApplyDateRangeFilter<TEntity>(
            IQueryable<TEntity> query,
            DateRangeFilter filter
            ) where TEntity : class
        {
            if (typeof(TEntity).GetProperty(filter.PropertyName) != null)
            {
                query = query.Where(e => EF.Property<DateTime>(e, filter.PropertyName) >= filter.StartDate);
                query = query.Where(e => EF.Property<DateTime>(e, filter.PropertyName) <= filter.EndDate);
            }

            return query;
        }

    }
}
