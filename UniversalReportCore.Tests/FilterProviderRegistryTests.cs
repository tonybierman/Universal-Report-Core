using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class FilterProviderRegistryTests
    {
        public interface ITestFilterProvider : IFilterProvider<string> { }

        [Fact]
        public void GetProvider_ShouldReturnProvider_WhenKeyExists()
        {
            // Arrange
            var mockProvider = new Mock<ITestFilterProvider>();
            mockProvider.Setup(p => p.Key).Returns("test-key");

            var registry = new FilterProviderRegistry<string>(new List<IFilterProvider<string>> { mockProvider.Object });

            // Act
            var result = registry.GetProvider("test-key");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test-key", result.Key);
        }

        [Fact]
        public void GetProvider_ShouldThrowKeyNotFoundException_WhenKeyDoesNotExist()
        {
            // Arrange
            var registry = new FilterProviderRegistry<string>(new List<IFilterProvider<string>>());

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => registry.GetProvider("non-existent-key"));
            Assert.Contains("No filter provider registered with key 'non-existent-key'", exception.Message);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithMultipleProviders()
        {
            // Arrange
            var mockProvider1 = new Mock<ITestFilterProvider>();
            mockProvider1.Setup(p => p.Key).Returns("key1");

            var mockProvider2 = new Mock<ITestFilterProvider>();
            mockProvider2.Setup(p => p.Key).Returns("key2");

            var providers = new List<IFilterProvider<string>> { mockProvider1.Object, mockProvider2.Object };

            // Act
            var registry = new FilterProviderRegistry<string>(providers);

            // Assert
            Assert.NotNull(registry.GetProvider("key1"));
            Assert.NotNull(registry.GetProvider("key2"));
        }
    }
}
