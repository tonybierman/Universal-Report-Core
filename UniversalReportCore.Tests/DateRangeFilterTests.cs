using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Xunit;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Tests
{
    public class DateRangeFilterTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            DateTime start = new DateTime(2024, 1, 1);
            DateTime end = new DateTime(2024, 12, 31);
            string propertyName = "CreatedDate";

            // Act
            var filter = new DateRangeFilter(start, end, propertyName);

            // Assert
            Assert.Equal(start, filter.StartDate);
            Assert.Equal(end, filter.EndDate);
            Assert.Equal(propertyName, filter.PropertyName);
        }

        [Fact]
        public void HasValue_ReturnsTrue_WhenStartDateIsSet()
        {
            // Arrange
            var filter = new DateRangeFilter(new DateTime(2024, 1, 1), DateTime.MinValue, "CreatedDate");

            // Act & Assert
            Assert.True(filter.HasValue);
        }

        [Fact]
        public void HasValue_ReturnsTrue_WhenEndDateIsSet()
        {
            // Arrange
            var filter = new DateRangeFilter(DateTime.MinValue, new DateTime(2024, 12, 31), "UpdatedDate");

            // Act & Assert
            Assert.True(filter.HasValue);
        }

        [Fact]
        public void HasValue_ReturnsTrue_WhenBothDatesAreSet()
        {
            // Arrange
            var filter = new DateRangeFilter(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), "CreatedDate");

            // Act & Assert
            Assert.True(filter.HasValue);
        }

        [Fact]
        public void HasValue_ReturnsFalse_WhenNoDateIsSet()
        {
            // Arrange
            var filter = new DateRangeFilter(DateTime.MinValue, DateTime.MinValue, "CreatedDate");

            // Act & Assert
            Assert.False(filter.HasValue);
        }

        [Fact]
        public void HasValue_ReturnsFalse_WhenPropertyNameIsEmpty()
        {
            // Arrange
            var filter = new DateRangeFilter(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), "");

            // Act & Assert
            Assert.False(filter.HasValue);
        }

        [Fact]
        public void HasValue_ReturnsFalse_WhenPropertyNameIsWhitespace()
        {
            // Arrange
            var filter = new DateRangeFilter(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), "   ");

            // Act & Assert
            Assert.False(filter.HasValue);
        }

        [Fact]
        public void HasValue_ReturnsFalse_WhenPropertyNameIsNull()
        {
            // Arrange
            var filter = new DateRangeFilter(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), null);

            // Act & Assert
            Assert.False(filter.HasValue);
        }
    }

}
