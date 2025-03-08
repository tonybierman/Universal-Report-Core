using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using UniversalReportCore;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FilterFactoryTests
    {
        private class TestEntity
        {
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public string City { get; set; } = string.Empty;
        }

        private class TestFilterProvider : IFilterProvider<TestEntity>
        {
            private readonly List<Expression<Func<TestEntity, bool>>> _andFilters;
            private readonly List<Expression<Func<TestEntity, bool>>> _orFilters;

            public TestFilterProvider(List<Expression<Func<TestEntity, bool>>> andFilters, List<Expression<Func<TestEntity, bool>>> orFilters)
            {
                _andFilters = andFilters ?? new();
                _orFilters = orFilters ?? new();
            }

            public string Key => throw new NotImplementedException();

            public List<Expression<Func<TestEntity, bool>>> GetAndFilters() => _andFilters;
            public List<Expression<Func<TestEntity, bool>>> GetOrFilters() => _orFilters;

            IEnumerable<Expression<Func<TestEntity, bool>>> IFilterProvider<TestEntity>.GetAndFilters()
            {
                return GetAndFilters();
            }

            IEnumerable<Expression<Func<TestEntity, bool>>> IFilterProvider<TestEntity>.GetOrFilters()
            {
                return GetOrFilters();
            }
        }

        [Fact]
        public void BuildPredicate_ShouldApplyAndFiltersCorrectly()
        {
            // Arrange: AND filters - must be "Tom" AND Age > 18
            var andFilters = new List<Expression<Func<TestEntity, bool>>>
            {
                e => e.Name == "Tom",
                e => e.Age > 18
            };

            var provider = new TestFilterProvider(andFilters, new());
            var factory = new FilterFactory<TestEntity>();

            // Act
            var predicate = factory.BuildPredicate(provider);
            var compiled = predicate.Compile();

            // Assert
            Assert.True(compiled(new TestEntity { Name = "Tom", Age = 25 })); // ✅ Matches both conditions
            Assert.False(compiled(new TestEntity { Name = "Tom", Age = 17 })); // ❌ Age too low
            Assert.False(compiled(new TestEntity { Name = "Bob", Age = 25 })); // ❌ Wrong Name
        }

        [Fact]
        public void BuildPredicate_ShouldApplyOrFiltersCorrectly()
        {
            // Arrange: OR filters - must be from "New York" OR "Los Angeles"
            var orFilters = new List<Expression<Func<TestEntity, bool>>>
            {
                e => e.City == "New York",
                e => e.City == "Los Angeles"
            };

            var provider = new TestFilterProvider(new(), orFilters);
            var factory = new FilterFactory<TestEntity>();

            // Act
            var predicate = factory.BuildPredicate(provider);
            var compiled = predicate.Compile();

            // Assert
            Assert.True(compiled(new TestEntity { City = "New York" })); // ✅ Matches first OR condition
            Assert.True(compiled(new TestEntity { City = "Los Angeles" })); // ✅ Matches second OR condition
            Assert.False(compiled(new TestEntity { City = "Chicago" })); // ❌ Doesn't match either
        }

        [Fact]
        public void BuildPredicate_ShouldCombineAndOrFiltersCorrectly()
        {
            // Arrange: 
            // AND: Must be Age > 18
            // OR: Must be from "New York" OR "Los Angeles"
            var andFilters = new List<Expression<Func<TestEntity, bool>>>
            {
                e => e.Age > 18
            };

            var orFilters = new List<Expression<Func<TestEntity, bool>>>
            {
                e => e.City == "New York",
                e => e.City == "Los Angeles"
            };

            var provider = new TestFilterProvider(andFilters, orFilters);
            var factory = new FilterFactory<TestEntity>();

            // Act
            var predicate = factory.BuildPredicate(provider);
            var compiled = predicate.Compile();

            // Assert
            Assert.True(compiled(new TestEntity { Age = 25, City = "New York" })); // ✅ Matches both AND & OR
            Assert.True(compiled(new TestEntity { Age = 30, City = "Los Angeles" })); // ✅ Matches both AND & OR
            Assert.False(compiled(new TestEntity { Age = 25, City = "Chicago" })); // ❌ Passes AND but fails OR
            Assert.False(compiled(new TestEntity { Age = 17, City = "New York" })); // ❌ Passes OR but fails AND
        }
    }
}
