using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using UniversalReportCore;
using UniversalReportCore.Helpers;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Services.QueryPipeline;

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
        /// <param name="totalCount">Total count of record set.</param>
        /// <param name="query">An optional initial query. If null, it defaults to querying all records of <typeparamref name="TEntity"/>.</param>
        /// <returns>A paginated list of <typeparamref name="TViewModel"/> with aggregates and metadata.</returns>
        public async Task<PaginatedList<TViewModel>> GetPagedAsync<TEntity, TViewModel>(
                PagedQueryParameters<TEntity> parameters,
                int totalCount,
                IQueryable<TEntity>? query = null) where TEntity : class where TViewModel : class
        {
            var pipelineSteps = new List<IQueryPipelineStep<TEntity>>
            {
                new ReportFilterStep<TEntity>(),
                new FacetedFilterStep<TEntity>(),
                new DateRangeFilterStep<TEntity>(),
                new CohortFilterStep<TEntity>(),
                new SortingStep<TEntity>()

            };
            var stopwatch = Stopwatch.StartNew();

            // Initialize query if not provided
            query = query ?? _dbContext.Set<TEntity>().AsNoTracking();

            // Execute pipeline steps
            foreach (var step in pipelineSteps)
            {
                query = step.Execute(query, parameters);
            }

            // Ensure AggregateLogic is not null
            parameters.AggregateLogic ??= async src => new Dictionary<string, dynamic>();

            // Create paginated list with mapping and aggregates
            var result = await PaginatedList<TViewModel>.CreateMappedWithAggregatesAsync(
                query,
                parameters.PageIndex ?? 1,
                parameters.ItemsPerPage ?? 25, // TODO: Replace with configurable value
                entity => _mapper.Map<TViewModel>(entity),
                parameters.AggregateLogic,
                parameters.MetaLogic
            );

            // Set total count if not provided
            if (totalCount == 0)
            {
                totalCount = await query.CountAsync();
            }
            result.EnsureTotalItemsCount(totalCount);

            // Add query duration to metadata
            stopwatch.Stop();
            if (result.Meta != null)
            {
                result.Meta["QueryDuration"] = $"{stopwatch.Elapsed.TotalMilliseconds:N2} ms";
            }

            return result;
        }
    }
}
