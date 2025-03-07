using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using UniversalReport.Services;
using UniversalReportCore.Data;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Ui
{
    public class PageHelperBase<TEntity, TViewModel> : IReportPageHelper<TEntity, TViewModel>
        where TViewModel : class, new()
        where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly IUniversalReportService _reportService;
        protected readonly IReportColumnFactory _reportColumnFactory;
        protected readonly IQueryFactory<TEntity> _queryFactory;
        private readonly IFilterProviderRegistry<TEntity> _filterRegistry;
        private readonly FilterFactory<TEntity> _filterFactory;

        public string DefaultSort { get; set; }

        public PageHelperBase(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<TEntity> queryFactory,
            IFilterProviderRegistry<TEntity> filterRegistry,
            FilterFactory<TEntity> filterFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _reportService = reportService;
            _reportColumnFactory = reportColumnFactory;
            _queryFactory = queryFactory;
            _filterRegistry = filterRegistry;
            _filterFactory = filterFactory;
            DefaultSort = "Product.SkuDesc";
        }

        PagedQueryParametersBase IReportPageHelperBase.CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return CreateQueryParameters(queryType, columns, pageIndex, sort, ipp, cohortIds);
        }

        public virtual PagedQueryParameters<TEntity> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            throw new NotImplementedException();
        }

        protected virtual void EnsureUserFilter(PagedQueryParameters<TEntity> parameters)
        {
            if (parameters.FilterKeys != null && parameters.FilterKeys.Any())
            {
                var combinedPredicate = PredicateBuilder.New<TEntity>(true); // Start with 'false' for OR chaining

                foreach (var key in parameters.FilterKeys)
                {
                    var provider = _filterRegistry.GetProvider(key);
                    if (provider != null)
                    {
                        var predicate = _filterFactory.BuildPredicate(provider);
                        combinedPredicate = combinedPredicate.And(predicate); // Change to Or for OR chaining
                    }
                }

                parameters.AdditionalFilter = (query) =>
                {
                    return query.Where(combinedPredicate);
                };
            }
        }

        public virtual Task<PaginatedList<TViewModel>> GetPagedDataAsync(PagedQueryParameters<TEntity> parameters, int totalCount = 0)
        {
            throw new NotImplementedException();
        }

        public virtual List<IReportColumnDefinition> GetReportColumns(string slug)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ICohort[]?> GetCohortsAsync(int[] cohortIds)
        {
            return null;
        }

        public List<ChartDataPoint> GetChartData(IPaginatedList items, string key)
        {
            return _mapper.Map<List<ChartDataPoint>>(
                items,
                opts =>
                {
                    opts.Items["Key"] = key;
                }
            );
        }

        public ChartDataPoint GetChartDataTotals(Dictionary<string, dynamic> data, string key)
        {
            TViewModel item = MapDictionaryToObject(data);

            return _mapper.Map<ChartDataPoint>(
                item,
                opts =>
                {
                    opts.Items["Key"] = key;
                }
            );
        }

        public TViewModel MapDictionaryToObject(Dictionary<string, dynamic> data)
        {
            var obj = new TViewModel();
            var properties = typeof(TViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (data.TryGetValue(prop.Name, out var value) && value != null)
                {
                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                        // Convert the value to match the property type
                        var convertedValue = Convert.ChangeType(value, targetType);
                        prop.SetValue(obj, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (log, rethrow, or suppress as needed)
                        //Console.WriteLine($"Failed to map {prop.Name}: {ex.Message}");
                    }
                }
            }

            return obj;
        }

        #region IReportPageHelperBase

        public async Task<object> GetPagedDataAsync(PagedQueryParametersBase parameters, int totalCount = 0)
        {
            if (parameters is PagedQueryParameters<TEntity> typedParameters)
            {
                if (!typedParameters.ShouldAggregate)
                {
                    typedParameters.AggregateLogic = null;
                }

                var result = await GetPagedDataAsync(typedParameters, totalCount);
                return result;
            }
            throw new ArgumentException($"Invalid parameters type. Expected {typeof(PagedQueryParameters<TEntity>)}, received {parameters.GetType()}");
        }

        #endregion

    }
}
