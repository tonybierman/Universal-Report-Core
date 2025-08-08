using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Win32;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
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

        protected readonly IFilterProvider<TEntity> _filterProvider;
        protected readonly FilterFactory<TEntity> _filterFactory;

        public string DefaultSort { get; set; }

        public PageHelperBase(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<TEntity> queryFactory,
            IFilterProvider<TEntity> filterProvider,
            FilterFactory<TEntity> filterFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _reportService = reportService;
            _reportColumnFactory = reportColumnFactory;
            _queryFactory = queryFactory;
            _filterProvider = filterProvider;
            _filterFactory = filterFactory;
            DefaultSort = "Product.SkuDesc";
        }

        public virtual bool HasFilters()
        {
            return false;
        }

        /// <summary>
        /// Determines whether any column property names intersect with the filter provider's facet keys.
        /// </summary>
        /// <param name="columns">The list of column definitions to check for filter intersection.</param>
        /// <returns><c>true</c> if there is any overlap between column PropertyNames and facet keys; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="columns"/> or <see cref="_filterProvider"/> is null.</exception>
        public bool HasFilters(List<IReportColumnDefinition> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (_filterProvider == null)
                throw new ArgumentNullException(nameof(_filterProvider));

            var facetKeys = _filterProvider.GetFacetKeys();
            if (facetKeys == null || !facetKeys.Keys.Any())
                return false;

            // Check intersection of facet keys and column PropertyNames
            return facetKeys.Keys.Intersect(columns.Select(a => a.PropertyName)).Any();
        }

        protected virtual FilterConfig<TEntity>? EnsureFilterConfig(string[]? filterKeys)
        {
            if(filterKeys == null || filterKeys.Length == 0)
            {
                // If no filter keys are provided, return null
                return null;
            }

            return new FilterConfig<TEntity>(_filterProvider, _filterFactory, filterKeys);
        }

        PagedQueryParametersBase IReportPageHelperBase.CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds, string[]? filterKeys)
        {
            return CreateQueryParameters(queryType, columns, pageIndex, sort, ipp, cohortIds, filterKeys);
        }

        public virtual PagedQueryParameters<TEntity> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds, string[]? filterKeys)
        {
            throw new NotImplementedException();
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

        protected virtual void EnsureFacetedFilter(PagedQueryParameters<TEntity> parameters)
        {
            var predicate = _filterFactory.BuildPredicate(parameters.FilterKeys);

            parameters.FacetedFilter = (query) =>
            {
                return query.Where(predicate);
            };
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
                EnsureFacetedFilter(typedParameters);
                var result = await GetPagedDataAsync(typedParameters, totalCount);

                return result;
            }
            throw new ArgumentException($"Invalid parameters type. Expected {typeof(PagedQueryParameters<TEntity>)}, received {parameters.GetType()}");
        }

        public List<(string Heading, List<SelectListItem> Options)> GetFilterSelectList(string[]? selectedKeys)
        {
            return _filterProvider
                .GetFacetKeys()
                .Select(facet => (
                    Heading: facet.Key, // The UI-friendly heading (e.g., "Country", "Gender")
                    Options: facet.Value
                        .Where(key => _filterProvider.Filters.ContainsKey(key)) // Ensure the key exists
                        .OrderBy(key => key)
                        .Select(key => new SelectListItem
                        {
                            Text = _filterProvider.Facets.SelectMany(f => f.Values)
                                .FirstOrDefault(v => v.Key == key)?.DisplayName ?? key, // Use DisplayName if available
                            Value = key,
                            Selected = selectedKeys?.Contains(key) == true
                        })
                        .ToList()
                )).ToList();
        }


        public IFilterProviderBase FilterProvider { get => _filterProvider; }

        #endregion

        private string ExtractFilterLabel(Expression<Func<TEntity, bool>> filter)
        {
            if (filter.Body is BinaryExpression binaryExpr &&
                binaryExpr.Right is ConstantExpression constant)
            {
                return constant.Value?.ToString() ?? "";
            }
            return filter.ToString();
        }

        private string SerializeExpression(Expression<Func<TEntity, bool>> filter)
        {
            return filter.Body.ToString(); // Unique string for passing in query params
        }

    }
}
