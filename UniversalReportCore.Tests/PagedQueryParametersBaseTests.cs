using System;
using UniversalReportCore.PagedQueries;
using Xunit;
using FluentAssertions;

namespace UniversalReportCore.Tests
{
    public class PagedQueryParametersBaseTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var parameters = new PagedQueryParametersBase();

            // Assert
            parameters.DisplayKey.Should().BeNull();
            parameters.PageIndex.Should().BeNull();
            parameters.Sort.Should().BeNull();
            parameters.DateFilter.Should().BeNull();
            parameters.ItemsPerPage.Should().BeNull();
            parameters.CohortIds.Should().BeNull();
            parameters.ShouldAggregate.Should().BeFalse();
            parameters.FilterKeys.Should().BeNull();
        }

        [Fact]
        public void Properties_ShouldBeSettableAndRetainValues()
        {
            // Arrange
            var dateFilter = new DateRangeFilter();
            var cohortIds = new[] { 1, 2, 3 };
            var filterKeys = new[] { "Key1", "Key2" };

            // Act
            var parameters = new PagedQueryParametersBase
            {
                DisplayKey = "SKU123",
                PageIndex = 1,
                Sort = "ColumnNameDesc",
                DateFilter = dateFilter,
                ItemsPerPage = 50,
                CohortIds = cohortIds,
                ShouldAggregate = true,
                FilterKeys = filterKeys
            };

            // Assert
            parameters.DisplayKey.Should().Be("SKU123");
            parameters.PageIndex.Should().Be(1);
            parameters.Sort.Should().Be("ColumnNameDesc");
            parameters.DateFilter.Should().Be(dateFilter);
            parameters.ItemsPerPage.Should().Be(50);
            parameters.CohortIds.Should().BeEquivalentTo(new[] { 1, 2, 3 });
            parameters.ShouldAggregate.Should().BeTrue();
            parameters.FilterKeys.Should().BeEquivalentTo(new[] { "Key1", "Key2" });
        }

        [Fact]
        public void Properties_ShouldAllowNullValues()
        {
            // Arrange
            var parameters = new PagedQueryParametersBase
            {
                DisplayKey = null,
                PageIndex = null,
                Sort = null,
                DateFilter = null,
                ItemsPerPage = null,
                CohortIds = null,
                FilterKeys = null
            };

            // Assert
            parameters.DisplayKey.Should().BeNull();
            parameters.PageIndex.Should().BeNull();
            parameters.Sort.Should().BeNull();
            parameters.DateFilter.Should().BeNull();
            parameters.ItemsPerPage.Should().BeNull();
            parameters.CohortIds.Should().BeNull();
            parameters.FilterKeys.Should().BeNull();
        }
    }
}
