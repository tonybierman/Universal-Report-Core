using System;
using System.Linq.Expressions;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FacetValueTests
    {
        /// <summary>
        /// Verifies that the FacetValue constructor correctly initializes the key and filter properties.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            string key = "TestKey";
            Expression<Func<int, bool>> filterExpression = x => x > 10;

            // Act
            var facetValue = new FacetValue<int>(key, filterExpression);

            // Assert
            Assert.Equal(key, facetValue.Key);
            Assert.Equal(filterExpression, facetValue.Filter);
        }

        /// <summary>
        /// Ensures that the filter expression works as expected.
        /// </summary>
        [Fact]
        public void FilterExpression_ShouldEvaluateCorrectly()
        {
            // Arrange
            var facetValue = new FacetValue<int>("GreaterThan10", x => x > 10);
            var filterFunc = facetValue.Filter.Compile();

            // Act & Assert
            Assert.False(filterFunc(5));   // 5 is not greater than 10
            Assert.True(filterFunc(15));  // 15 is greater than 10
        }

        /// <summary>
        /// Ensures that passing a null key is not allowed.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenValuesIsNull()
        {
            // Arrange
            string facetName = "Category";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Facet<int>(facetName, null));
        }

        /// <summary>
        /// Ensures that passing a null filter throws an exception.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenFilterIsNull()
        {
            // Arrange
            string key = "TestKey";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FacetValue<int>(key, null));
        }
    }
}
