using System;
using System.Collections.Generic;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FacetTests
    {
        /// <summary>
        /// Verifies that the Facet constructor correctly initializes the name and values properties.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            string facetName = "Category";
            var values = new List<FacetValue<int>>
            {
                new FacetValue<int>("GreaterThan10", x => x > 10),
                new FacetValue<int>("LessThan5", x => x < 5)
            };

            // Act
            var facet = new Facet<int>(facetName, values);

            // Assert
            Assert.Equal(facetName, facet.Name);
            Assert.Equal(values, facet.Values);
            Assert.Equal(2, facet.Values.Count);
        }

        /// <summary>
        /// Ensures that the constructor correctly assigns an empty list when given an empty collection.
        /// </summary>
        [Fact]
        public void Constructor_ShouldHandleEmptyValuesList()
        {
            // Arrange
            string facetName = "Category";
            var values = new List<FacetValue<int>>();

            // Act
            var facet = new Facet<int>(facetName, values);

            // Assert
            Assert.Equal(facetName, facet.Name);
            Assert.Empty(facet.Values);
        }

        /// <summary>
        /// Ensures that passing a null name is not allowed.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenFilterIsNull()
        {
            // Arrange
            string key = "TestKey";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FacetValue<int>(key, null));
        }


        /// <summary>
        /// Ensures that passing a null values list throws an exception.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenValuesIsNull()
        {
            // Arrange
            string facetName = "Category";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Facet<int>(facetName, null));
        }
    }
}
