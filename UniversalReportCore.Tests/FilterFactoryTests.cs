using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LinqKit;
using Moq;
using UniversalReportCore;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FilterFactoryTests
    {
        public class TestEntity2
        {
            public string Category { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
        }

        private readonly Mock<IFilterProvider<TestEntity2>> _mockProvider;
        private readonly FilterFactory<TestEntity2> _filterFactory;

        public FilterFactoryTests()
        {
            _mockProvider = new Mock<IFilterProvider<TestEntity2>>();

            // Simulated filters
            var filters = new Dictionary<string, Expression<Func<TestEntity2, bool>>>
            {
                { "Canada", e => e.Category == "Canada" },
                { "Mexico", e => e.Category == "Mexico" },
                { "Male", e => e.Gender == "Male" },
                { "Female", e => e.Gender == "Female" }
            };

            var facetKeys = new Dictionary<string, List<string>>
            {
                { "Country", new List<string> { "Canada", "Mexico" } },
                { "Gender", new List<string> { "Male", "Female" } }
            };

            _mockProvider.Setup(p => p.Filters).Returns(filters);
            _mockProvider.Setup(p => p.GetFacetKeys()).Returns(facetKeys);

            _filterFactory = new FilterFactory<TestEntity2>(_mockProvider.Object);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FilterFactory<TestEntity2>(null!));
        }

        [Fact]
        public void BuildPredicate_ShouldReturnTruePredicate_WhenNoFiltersSelected()
        {
            var predicate = _filterFactory.BuildPredicate(new List<string>());
            var testEntity = new TestEntity2 { Category = "Canada", Gender = "Male" };

            Assert.True(predicate.Compile().Invoke(testEntity));
        }

        [Fact]
        public void BuildPredicate_ShouldFilterBySingleKey()
        {
            var predicate = _filterFactory.BuildPredicate(new List<string> { "Canada" });
            var match = new TestEntity2 { Category = "Canada", Gender = "Male" };
            var noMatch = new TestEntity2 { Category = "Mexico", Gender = "Male" };

            Assert.True(predicate.Compile().Invoke(match));
            Assert.False(predicate.Compile().Invoke(noMatch));
        }

        [Fact]
        public void BuildPredicate_ShouldApplyOrWithinSameFacet()
        {
            var predicate = _filterFactory.BuildPredicate(new List<string> { "Canada", "Mexico" });

            var match1 = new TestEntity2 { Category = "Canada", Gender = "Male" };
            var match2 = new TestEntity2 { Category = "Mexico", Gender = "Male" };
            var noMatch = new TestEntity2 { Category = "USA", Gender = "Male" };

            Assert.True(predicate.Compile().Invoke(match1));
            Assert.True(predicate.Compile().Invoke(match2));
            Assert.False(predicate.Compile().Invoke(noMatch));
        }

        [Fact]
        public void BuildPredicate_ShouldApplyAndAcrossDifferentFacets()
        {
            var predicate = _filterFactory.BuildPredicate(new List<string> { "Canada", "Male" });

            var match = new TestEntity2 { Category = "Canada", Gender = "Male" };
            var noMatch1 = new TestEntity2 { Category = "Canada", Gender = "Female" };
            var noMatch2 = new TestEntity2 { Category = "Mexico", Gender = "Male" };

            Assert.True(predicate.Compile().Invoke(match));
            Assert.False(predicate.Compile().Invoke(noMatch1));
            Assert.False(predicate.Compile().Invoke(noMatch2));
        }

        [Fact]
        public void BuildPredicate_ShouldIgnoreInvalidKeys()
        {
            var predicate = _filterFactory.BuildPredicate(new List<string> { "Canada", "InvalidKey" });

            var match = new TestEntity2 { Category = "Canada", Gender = "Male" };
            var noMatch = new TestEntity2 { Category = "Mexico", Gender = "Male" };

            Assert.True(predicate.Compile().Invoke(match));
            Assert.False(predicate.Compile().Invoke(noMatch));
        }
    }
}
