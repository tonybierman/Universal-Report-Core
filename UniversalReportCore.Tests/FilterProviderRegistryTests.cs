using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using UniversalReportCore;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FilterProviderRegistryTests
    {
        public class TestEntity1 { }

        private readonly Mock<IFilterProvider<TestEntity1>> _mockProvider1;
        private readonly Mock<IFilterProvider<TestEntity1>> _mockProvider2;

        public FilterProviderRegistryTests()
        {
            _mockProvider1 = new Mock<IFilterProvider<TestEntity1>>();
            _mockProvider2 = new Mock<IFilterProvider<TestEntity1>>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenProvidersAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FilterProviderRegistry<TestEntity1>(null!));
        }

        [Fact]
        public void GetProvider_ShouldReturnFirstProvider_WhenProvidersExist()
        {
            var providers = new List<IFilterProvider<TestEntity1>> { _mockProvider1.Object, _mockProvider2.Object };
            var registry = new FilterProviderRegistry<TestEntity1>(providers);

            var provider = registry.GetProvider();

            Assert.NotNull(provider);
            Assert.Contains(provider, providers);
        }

        [Fact]
        public void GetProvider_ShouldThrowInvalidOperationException_WhenNoProvidersExist()
        {
            var registry = new FilterProviderRegistry<TestEntity1>(new List<IFilterProvider<TestEntity1>>());

            var exception = Assert.Throws<InvalidOperationException>(() => registry.GetProvider());
            Assert.Equal($"No filter provider registered for entity type '{typeof(TestEntity1).Name}'.", exception.Message);
        }

        [Fact]
        public void GetAllProviders_ShouldReturnAllProviders_WhenProvidersExist()
        {
            var providers = new List<IFilterProvider<TestEntity1>> { _mockProvider1.Object, _mockProvider2.Object };
            var registry = new FilterProviderRegistry<TestEntity1>(providers);

            var result = registry.GetAllProviders();

            Assert.Equal(providers.Count, result.Count());
            Assert.All(providers, provider => Assert.Contains(provider, result));
        }

        [Fact]
        public void GetAllProviders_ShouldReturnEmptyCollection_WhenNoProvidersExist()
        {
            var registry = new FilterProviderRegistry<TestEntity1>(new List<IFilterProvider<TestEntity1>>());

            var result = registry.GetAllProviders();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
