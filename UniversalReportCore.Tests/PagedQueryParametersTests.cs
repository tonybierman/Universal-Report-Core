using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using UniversalReportCore.PagedQueries;
using UniversalReportCore;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportDemo.Data;
using UniversalReportDemo.Reports.CityPop;

namespace UniversalReportCore.Tests
{

    public class PagedQueryParametersTests
    {
        private readonly IReportColumnDefinition[] _columns = new IReportColumnDefinition[]
        {
        new ReportColumnDefinition { PropertyName = "City", DisplayName = "City", IsSortable = true },
        new ReportColumnDefinition { PropertyName = "Population", DisplayName = "Population", IsSortable = true }
        };

        [Fact]
        public void Constructor_AssignsValuesCorrectly()
        {
            // Arrange
            int? pageIndex = 2;
            string? sort = "Population";
            int? itemsPerPage = 20;
            int[]? cohortIds = new[] { 1, 2, 3 };

            // Act
            var queryParameters = new PagedCityPopulationQueryParameters(_columns, pageIndex, sort, itemsPerPage, cohortIds);

            // Assert
            Assert.Equal(pageIndex, queryParameters.PageIndex);
            Assert.Equal(sort, queryParameters.Sort);
            Assert.Equal(itemsPerPage, queryParameters.ItemsPerPage);
            Assert.Equal(cohortIds, queryParameters.CohortIds);
            Assert.Equal(_columns, queryParameters.ReportColumns);
        }

        [Fact]
        public async Task AggregateLogic_CorrectlyExecutes()
        {
            // Arrange
            var data = new List<CityPopulation>
        {
            new CityPopulation { City = "New York", Value = 8000000 },
            new CityPopulation { City = "Los Angeles", Value = 4000000 },
            new CityPopulation { City = "Chicago", Value = 2700000 }
        }.AsQueryable();

            Func<IQueryable<CityPopulation>, Task<Dictionary<string, dynamic>>> aggregateLogic = async (query) =>
            {
                return await Task.FromResult(new Dictionary<string, dynamic>
            {
                { "TotalPopulation", query.Sum(x => x.Value) }
            });
            };

            var queryParameters = new PagedCityPopulationQueryParameters(_columns, 1, "Population", 10, null, null, aggregateLogic, null);

            // Act
            var result = await queryParameters.AggregateLogic!(data);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("TotalPopulation"));
            Assert.Equal(14700000, result["TotalPopulation"]);
        }

        [Fact]
        public void AdditionalFilter_CorrectlyFiltersData()
        {
            // Arrange
            var data = new List<CityPopulation>
        {
            new CityPopulation { City = "New York", Value = 8000000 },
            new CityPopulation { City = "Los Angeles", Value = 4000000 },
            new CityPopulation { City = "Chicago", Value = 2700000 }
        }.AsQueryable();

            Func<IQueryable<CityPopulation>, IQueryable<CityPopulation>> filterLogic = (query) =>
                query.Where(x => x.Value > 5000000);

            var queryParameters = new PagedCityPopulationQueryParameters(_columns, 1, "Population", 10, null, filterLogic, null, null);

            // Act
            var filteredResult = queryParameters.AdditionalFilter!(data).ToList();

            // Assert
            Assert.NotNull(filteredResult);
            Assert.Single(filteredResult);
            Assert.Equal("New York", filteredResult[0].City);
        }

        [Fact]
        public async Task MetaLogic_CorrectlyExecutes()
        {
            // Arrange
            var data = new List<CityPopulation>
        {
            new CityPopulation { City = "New York", Value = 8000000 },
            new CityPopulation { City = "Los Angeles", Value = 4000000 },
            new CityPopulation { City = "Chicago", Value = 2700000 }
        }.AsQueryable();

            Func<IQueryable<CityPopulation>, Task<Dictionary<string, dynamic>>> metaLogic = async (query) =>
            {
                return await Task.FromResult(new Dictionary<string, dynamic>
            {
                { "TotalCities", query.Count() }
            });
            };

            var queryParameters = new PagedCityPopulationQueryParameters(_columns, 1, "Population", 10, null, null, null, metaLogic);

            // Act
            var result = await queryParameters.MetaLogic!(data);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("TotalCities"));
            Assert.Equal(3, result["TotalCities"]);
        }
    }

}
