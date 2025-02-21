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
using UniversalReportDemo.Data;
using Xunit;

namespace UniversalReportCore.Tests
{

    namespace UniversalReportCore.Tests.PagedQueries
    {
        public class QueryFactoryTests
        {
            private class MockPagedQueryProvider : IPagedQueryProvider<CityPopulation>
            {
                public string Slug => "CityPopulationDemo";

                public IQueryable<CityPopulation> EnsureAggregateQuery(IQueryable<CityPopulation> query, int[]? cohortIds)
                {
                    throw new NotImplementedException();
                }

                public IQueryable<CityPopulation> EnsureCohortQuery(IQueryable<CityPopulation> query, int cohortId)
                {
                    throw new NotImplementedException();
                }

                public PagedQueryParameters<CityPopulation> GetQuery(
                    IReportColumnDefinition[] columns,
                    int? pageIndex,
                    string? sort,
                    int? ipp,
                    int[]? cohortIds)
                {
                    return new PagedQueryParameters<CityPopulation>(columns, pageIndex, sort, ipp, cohortIds);
                }
            }

            [Fact]
            public void CreateQueryParameters_ValidSlug_ReturnsQueryParameters()
            {
                // Arrange
                var providers = new List<IPagedQueryProvider<CityPopulation>> { new MockPagedQueryProvider() };
                var queryFactory = new QueryFactory<CityPopulation>(providers);
                var columns = new IReportColumnDefinition[] { };

                // Act
                var result = queryFactory.CreateQueryParameters("CityPopulationDemo", columns, 1, "YearAsc", 50, new int[] { 1, 2, 3 });

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.PageIndex);
                Assert.Equal("YearAsc", result.Sort);
                Assert.Equal(50, result.ItemsPerPage);
                Assert.Equal(new int[] { 1, 2, 3 }, result.CohortIds);
            }

            [Fact]
            public void CreateQueryParameters_InvalidSlug_ThrowsException()
            {
                // Arrange
                var providers = new List<IPagedQueryProvider<CityPopulation>> { new MockPagedQueryProvider() };
                var queryFactory = new QueryFactory<CityPopulation>(providers);
                var columns = new IReportColumnDefinition[] { };

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() =>
                    queryFactory.CreateQueryParameters("InvalidSlug", columns, 1, "YearAsc", 50, new int[] { 1, 2, 3 }));

                Assert.Equal("Unsupported query type: InvalidSlug", ex.Message);
            }
        }
    }

}
