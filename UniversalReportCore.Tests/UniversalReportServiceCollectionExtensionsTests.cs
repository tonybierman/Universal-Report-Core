using Microsoft.Extensions.DependencyInjection;
using UniversalReportCore.Ui;
using UniversalReport.Services;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using Xunit;
using UniversalReportCore;
using UniversalReportCore.ViewModels;
using UniversalReportCore.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace UniversalReportCore.Tests
{
    public class UniversalReportServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddUniversalReport_ShouldRegisterCoreServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddUniversalReport();
            var provider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(provider.GetService<IPageMetaFactory>());
            Assert.NotNull(provider.GetService<IReportColumnFactory>());
        }

        [Fact]
        public void AddEntityReportServices_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddEntityReportServices<
                TestEntity,
                TestViewModel,
                TestQueryFactory,
                TestPageMetaProvider,
                TestReportColumnProvider,
                TestQueryProvider,
                TestPageHelper,
                TestFilterProvider>();

            var provider = services.BuildServiceProvider();

            // Assert: Verify that all dependencies are registered
            Assert.NotNull(provider.GetService<IQueryFactory<TestEntity>>());
            Assert.NotNull(provider.GetService<IPageMetaProvider>());
            Assert.NotNull(provider.GetService<IReportColumnProvider>());
            Assert.NotNull(provider.GetService<IPagedQueryProvider<TestEntity>>());
            Assert.NotNull(provider.GetService<IReportPageHelper<TestEntity, TestViewModel>>());
            Assert.NotNull(provider.GetService<IFilterProvider<TestEntity>>());
            Assert.NotNull(provider.GetService<FilterFactory<TestEntity>>());
        }
    }

    // Test Classes for Dependency Injection
    public class TestEntity { }
    public class TestViewModel { }
    public class TestQueryFactory : IQueryFactory<TestEntity>
    {
        public PagedQueryParameters<TestEntity> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            throw new NotImplementedException();
        }
    }
    public class TestPageMetaProvider : IPageMetaProvider
    {
        public string Slug => throw new NotImplementedException();

        public string CategorySlug => throw new NotImplementedException();

        public ChartMetaViewModel? GetChartMeta()
        {
            throw new NotImplementedException();
        }

        public PageMetaViewModel GetPageMeta()
        {
            throw new NotImplementedException();
        }

        string? IPageMetaProvider.GetActionWellPartial()
        {
            throw new NotImplementedException();
        }
    }
    public class TestReportColumnProvider : IReportColumnProvider
    {
        public string Slug => throw new NotImplementedException();

        public List<IReportColumnDefinition> GetColumns()
        {
            throw new NotImplementedException();
        }
    }
    public class TestQueryProvider : IPagedQueryProvider<TestEntity>
    {
        public string Slug => throw new NotImplementedException();

        PagedQueryParameters<TestEntity> IPagedQueryProvider<TestEntity>.BuildPagedQuery(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds, IQueryable<TestEntity>? reportQuery)
        {
            throw new NotImplementedException();
        }

        IQueryable<TestEntity> IPagedQueryProvider<TestEntity>.EnsureAggregatePredicate(IQueryable<TestEntity> query, int[]? cohortIds)
        {
            throw new NotImplementedException();
        }

        IQueryable<TestEntity> IPagedQueryProvider<TestEntity>.EnsureCohortPredicate(IQueryable<TestEntity> query, int cohortId)
        {
            throw new NotImplementedException();
        }

        IQueryable<TestEntity>? IPagedQueryProvider<TestEntity>.EnsureReportQuery()
        {
            throw new NotImplementedException();
        }
    }
    public class TestPageHelper : IReportPageHelper<TestEntity, TestViewModel>
    {
        public string DefaultSort => throw new NotImplementedException();

        public IFilterProviderBase FilterProvider => throw new NotImplementedException();

        public PagedQueryParameters<TestEntity> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            throw new NotImplementedException();
        }

        public List<ChartDataPoint> GetChartData(IPaginatedList items, string key)
        {
            throw new NotImplementedException();
        }

        public Task<ICohort[]?> GetCohortsAsync(int[] cohortIds)
        {
            throw new NotImplementedException();
        }

        public List<(string Heading, List<SelectListItem> Options)> GetFilterSelectList(string[]? keys)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<TestViewModel>> GetPagedDataAsync(PagedQueryParameters<TestEntity> parameters, int totalCount = 0)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetPagedDataAsync(PagedQueryParametersBase parameters, int totalCount = 0)
        {
            throw new NotImplementedException();
        }

        public List<IReportColumnDefinition> GetReportColumns(string slug)
        {
            throw new NotImplementedException();
        }

        public TestViewModel MapDictionaryToObject(Dictionary<string, dynamic> data)
        {
            throw new NotImplementedException();
        }

        PagedQueryParametersBase IReportPageHelperBase.CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return CreateQueryParameters(queryType, columns, pageIndex, sort, ipp, cohortIds);
        }

        bool IReportPageHelperBase.HasFilters(List<IReportColumnDefinition> columns)
        {
            throw new NotImplementedException();
        }
    }
    public class TestFilterProvider : IFilterProvider<TestEntity>
    {
        public Dictionary<string, Expression<Func<TestEntity, bool>>> Filters => throw new NotImplementedException();

        List<Facet<TestEntity>> IFilterProvider<TestEntity>.Facets => throw new NotImplementedException();

        public Dictionary<string, List<string>> GetFacetKeys()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<TestEntity, bool>> GetFilter(string key)
        {
            throw new NotImplementedException();
        }
    }
}
