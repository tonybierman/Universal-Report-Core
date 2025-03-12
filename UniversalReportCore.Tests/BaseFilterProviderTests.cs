using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using FluentAssertions;

namespace UniversalReportCore.Tests
{
    public class BaseFilterProviderTests
    {
        private class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class TestFilterProvider : BaseFilterProvider<TestEntity>
        {
            public TestFilterProvider(List<Facet<TestEntity>> facets) : base(facets) { }
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenFacetsAreNull()
        {
            Action act = () => new TestFilterProvider(null);
            act.Should().Throw<ArgumentNullException>().WithMessage("*facets*");
        }

        [Fact]
        public void Filters_ShouldReturnCorrectDictionary_WhenFacetsProvided()
        {
            // Arrange
            var facets = new List<Facet<TestEntity>>
            {
                new Facet<TestEntity>("Category1", new List<FacetValue<TestEntity>>
                {
                    new FacetValue<TestEntity>("Filter1", e => e.Id > 10),
                    new FacetValue<TestEntity>("Filter2", e => e.Name.StartsWith("A"))
                }),
                new Facet<TestEntity>("Category2", new List<FacetValue<TestEntity>>
                {
                    new FacetValue<TestEntity>("Filter3", e => e.Name.Contains("Test"))
                })
            };

            var provider = new TestFilterProvider(facets);

            // Act
            var filters = provider.Filters;

            // Assert
            filters.Should().HaveCount(3);
            filters.Should().ContainKey("Filter1").WhoseValue.Should().BeEquivalentTo((Expression<Func<TestEntity, bool>>)(e => e.Id > 10));
            filters.Should().ContainKey("Filter2").WhoseValue.Should().BeEquivalentTo((Expression<Func<TestEntity, bool>>)(e => e.Name.StartsWith("A")));
            filters.Should().ContainKey("Filter3").WhoseValue.Should().BeEquivalentTo((Expression<Func<TestEntity, bool>>)(e => e.Name.Contains("Test")));
        }

        [Fact]
        public void GetFacetKeys_ShouldReturnCorrectDictionary_WhenFacetsProvided()
        {
            // Arrange
            var facets = new List<Facet<TestEntity>>
            {
                new Facet<TestEntity>("Category1", new List<FacetValue<TestEntity>>
                {
                    new FacetValue<TestEntity>("Filter1", e => e.Id > 10),
                    new FacetValue<TestEntity>("Filter2", e => e.Name.StartsWith("A"))
                }),
                new Facet<TestEntity>("Category2", new List<FacetValue<TestEntity>>
                {
                    new FacetValue<TestEntity>("Filter3", e => e.Name.Contains("Test"))
                })
            };

            var provider = new TestFilterProvider(facets);

            // Act
            var facetKeys = provider.GetFacetKeys();

            // Assert
            facetKeys.Should().HaveCount(2);
            facetKeys["Category1"].Should().BeEquivalentTo(new List<string> { "Filter1", "Filter2" });
            facetKeys["Category2"].Should().BeEquivalentTo(new List<string> { "Filter3" });
        }

        [Fact]
        public void GetFilter_ShouldReturnCorrectExpression_WhenKeyExists()
        {
            // Arrange
            var facets = new List<Facet<TestEntity>>
            {
                new Facet<TestEntity>("Category1", new List<FacetValue<TestEntity>>
                {
                    new FacetValue<TestEntity>("Filter1", e => e.Id > 10)
                })
            };

            var provider = new TestFilterProvider(facets);

            // Act
            var filter = provider.GetFilter("Filter1");

            // Assert
            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo((Expression<Func<TestEntity, bool>>)(e => e.Id > 10));
        }

        [Fact]
        public void GetFilter_ShouldThrowKeyNotFoundException_WhenKeyDoesNotExist()
        {
            // Arrange
            var facets = new List<Facet<TestEntity>> { };
            var provider = new TestFilterProvider(facets);

            // Act
            Action act = () => provider.GetFilter("InvalidKey");

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}
