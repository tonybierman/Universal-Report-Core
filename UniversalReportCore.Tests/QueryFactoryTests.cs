using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using Xunit;
using UniversalReportCoreTests.Data;

namespace UniversalReportCore.Tests
{
    public class QueryFactoryTests
    {
        private class MockPagedQueryProvider : IPagedQueryProvider<Widget>
        {
            public string Slug => "CityPopulationDemo";

            public IQueryable<Widget> EnsureAggregatePredicate(IQueryable<Widget> query, int[]? cohortIds)
            {
                throw new NotImplementedException();
            }

            public IQueryable<Widget> EnsureCohortPredicate(IQueryable<Widget> query, int cohortId)
            {
                throw new NotImplementedException();
            }

            public PagedQueryParameters<Widget> BuildPagedQuery(
                IReportColumnDefinition[] columns,
                int? pageIndex,
                string? sort,
                int? ipp,
                int[]? cohortIds,
                IQueryable<Widget>? reportQuery)
            {
                return new PagedQueryParameters<Widget>(columns, pageIndex, sort, ipp, cohortIds, null);
            }

            IQueryable<Widget>? IPagedQueryProvider<Widget>.EnsureReportQuery()
            {
                return null;
            }

            public PagedQueryParameters<Widget> BuildPagedQuery(PreQueryArguments preQueryArgs, FilterConfig<Widget>? filterConfig = null, IQueryable<Widget>? reportQuery = null)
            {
                throw new NotImplementedException();
            }
        }

        //[Fact]
        //public void CreateQueryParameters_ValidSlug_ReturnsQueryParameters()
        //{
        //    // Arrange
        //    var providers = new List<IPagedQueryProvider<Widget>> { new MockPagedQueryProvider() };
        //    var queryFactory = new QueryFactory<Widget>(providers);
        //    var columns = new IReportColumnDefinition[] { };

        //    // Act
        //    var result = queryFactory.CreateQueryParameters("CityPopulationDemo", columns, 1, "YearAsc", 50, new int[] { 1, 2, 3 });

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(1, result.PageIndex);
        //    Assert.Equal("YearAsc", result.Sort);
        //    Assert.Equal(50, result.ItemsPerPage);
        //    Assert.Equal(new int[] { 1, 2, 3 }, result.CohortIds);
        //}

        //[Fact]
        //public void CreateQueryParameters_InvalidSlug_ThrowsException()
        //{
        //    // Arrange
        //    var providers = new List<IPagedQueryProvider<Widget>> { new MockPagedQueryProvider() };
        //    var queryFactory = new QueryFactory<Widget>(providers);
        //    var columns = new IReportColumnDefinition[] { };

        //    // Act & Assert
        //    var ex = Assert.Throws<InvalidOperationException>(() =>
        //        queryFactory.CreateQueryParameters("InvalidSlug", columns, 1, "YearAsc", 50, new int[] { 1, 2, 3 }));

        //    Assert.Equal("Unsupported query type: InvalidSlug", ex.Message);
        //}
    }
}
