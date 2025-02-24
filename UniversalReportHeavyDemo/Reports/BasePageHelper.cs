using AutoMapper;
using System.Reflection;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.Helpers;
using UniversalReportCore.PagedQueries;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports
{
    public class BasePageHelper<TEntity, TViewModel> : IReportPageHelper<TEntity, TViewModel>
        where TViewModel : class, new()
        where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly IUniversalReportService _reportService;
        protected readonly IReportColumnFactory _reportColumnFactory;
        protected readonly IQueryFactory<TEntity> _queryFactory;
        protected readonly ApplicationDbContext _dbContext;
        public string DefaultSort { get; set; }

        public BasePageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<TEntity> queryFactory,
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _reportService = reportService;
            _reportColumnFactory = reportColumnFactory;
            _queryFactory = queryFactory;
            _dbContext = dbContext;
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

        public virtual Task<PaginatedList<TViewModel>> GetPagedDataAsync(PagedQueryParameters<TEntity> parameters)
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

        public async Task<object> GetPagedDataAsync(PagedQueryParametersBase parameters)
        {
            if (parameters is PagedQueryParameters<TEntity> typedParameters)
            {
                var result = await GetPagedDataAsync(typedParameters);
                return result;
            }
            throw new ArgumentException($"Invalid parameters type. Expected {typeof(PagedQueryParameters<TEntity>)}, received {parameters.GetType()}");
        }

        //public virtual Task<List<IReportColumnDefinition>> GetReportColumns(string slug)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual Task<ICohort[]?> GetCohortsAsync(int[] cohortIds)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual Task<PaginatedList<TViewModel>> GetPagedDataAsync(PagedQueryParameters<TEntity> parameters)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

    }
}
