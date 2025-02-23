using System.Collections.Generic;
using System.Linq;
using Moq;
using UniversalReportCore;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportCore.Helpers;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class HardenedColumnSortTests
    {
        [Fact]
        public void Validate_ShouldReturnTrue_WhenSortColumnExists()
        {
            // Arrange
            var validSortColumn = "ValidColumn";

            var mockColumn = new Mock<IReportColumnDefinition>();
            mockColumn.Setup(c => c.PropertyName).Returns(validSortColumn);

            var reportColumns = new List<IReportColumnDefinition> { mockColumn.Object };

            var hardenedSort = new HardenedColumnSort(validSortColumn);

            // Act
            var result = hardenedSort.Validate(reportColumns);

            // Assert
            Assert.True(result);
            Assert.True(hardenedSort.IsValid);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenSortColumnDoesNotExist()
        {
            // Arrange
            var invalidSortColumn = "NonExistentColumn";

            var mockColumn = new Mock<IReportColumnDefinition>();
            mockColumn.Setup(c => c.PropertyName).Returns("ValidColumn"); // No match

            var reportColumns = new List<IReportColumnDefinition> { mockColumn.Object };

            var hardenedSort = new HardenedColumnSort(invalidSortColumn);

            // Act
            var result = hardenedSort.Validate(reportColumns);

            // Assert
            Assert.False(result);
            Assert.False(hardenedSort.IsValid);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenReportColumnsListIsEmpty()
        {
            // Arrange
            var sortColumn = "AnyColumn";
            var reportColumns = new List<IReportColumnDefinition>(); // Empty list

            var hardenedSort = new HardenedColumnSort(sortColumn);

            // Act
            var result = hardenedSort.Validate(reportColumns);

            // Assert
            Assert.False(result);
            Assert.False(hardenedSort.IsValid);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenSortColumnMatchesBaseSortKey()
        {
            // Arrange
            var sortColumnWithOrder = "ValidColumnDesc";
            var expectedBaseSortKey = "ValidColumn";

            var mockColumn = new Mock<IReportColumnDefinition>();
            mockColumn.Setup(c => c.PropertyName).Returns(expectedBaseSortKey);

            var reportColumns = new List<IReportColumnDefinition> { mockColumn.Object };

            var hardenedSort = new HardenedColumnSort(sortColumnWithOrder);

            // Act
            var result = hardenedSort.Validate(reportColumns);

            // Assert
            Assert.True(result);
            Assert.True(hardenedSort.IsValid);
        }
    }
}
