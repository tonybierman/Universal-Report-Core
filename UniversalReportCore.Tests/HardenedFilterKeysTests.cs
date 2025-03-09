using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.Tests
{
    public class HardenedFilterKeysTests
    {
        [Fact]
        public void Contains_ShouldReturnTrue_WhenKeyExists()
        {
            // Arrange
            var filterKeys = new HardenedFilterKeys(new[] { "Canada", "Male", "Mexico" });

            // Act
            bool containsCanada = filterKeys.Contains("Canada");
            bool containsMale = filterKeys.Contains("Male");

            // Assert
            Assert.True(containsCanada);
            Assert.True(containsMale);
        }

        [Fact]
        public void Contains_ShouldReturnFalse_WhenKeyDoesNotExist()
        {
            // Arrange
            var filterKeys = new HardenedFilterKeys(new[] { "Canada", "Male" });

            // Act
            bool containsJapan = filterKeys.Contains("Japan");

            // Assert
            Assert.False(containsJapan);
        }

        [Fact]
        public void Contains_ShouldReturnFalse_WhenValueIsNull()
        {
            // Arrange
            var filterKeys = new HardenedFilterKeys(null);

            // Act
            bool containsAny = filterKeys.Contains("Canada");

            // Assert
            Assert.False(containsAny);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenValueIsNull()
        {
            // Arrange
            var mockFilterProvider = new Mock<IFilterProviderBase>();
            mockFilterProvider.Setup(f => f.GetFacetKeys()).Returns(new Dictionary<string, List<string>>
        {
            { "Country", new List<string> { "Canada", "Mexico" } },
            { "Gender", new List<string> { "Male", "Female" } }
        });

            var filterKeys = new HardenedFilterKeys(null);

            // Act
            bool isValid = filterKeys.Validate(mockFilterProvider.Object);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenAllKeysAreValid()
        {
            // Arrange
            var mockFilterProvider = new Mock<IFilterProviderBase>();
            mockFilterProvider.Setup(f => f.GetFacetKeys()).Returns(new Dictionary<string, List<string>>
        {
            { "Country", new List<string> { "Canada", "Mexico" } },
            { "Gender", new List<string> { "Male", "Female" } }
        });

            var filterKeys = new HardenedFilterKeys(new[] { "Canada", "Male" });

            // Act
            bool isValid = filterKeys.Validate(mockFilterProvider.Object);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenAnyKeyIsInvalid()
        {
            // Arrange
            var mockFilterProvider = new Mock<IFilterProviderBase>();
            mockFilterProvider.Setup(f => f.GetFacetKeys()).Returns(new Dictionary<string, List<string>>
        {
            { "Country", new List<string> { "Canada", "Mexico" } },
            { "Gender", new List<string> { "Male", "Female" } }
        });

            var filterKeys = new HardenedFilterKeys(new[] { "Canada", "InvalidKey" });

            // Act
            bool isValid = filterKeys.Validate(mockFilterProvider.Object);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenKeysAreNullOrEmpty()
        {
            // Arrange
            var mockFilterProvider = new Mock<IFilterProviderBase>();
            mockFilterProvider.Setup(f => f.GetFacetKeys()).Returns(new Dictionary<string, List<string>>
        {
            { "Country", new List<string> { "Canada", "Mexico" } },
            { "Gender", new List<string> { "Male", "Female" } }
        });

            var filterKeys = new HardenedFilterKeys(Array.Empty<string>());

            // Act
            bool isValid = filterKeys.Validate(mockFilterProvider.Object);

            // Assert
            Assert.True(isValid);
        }
    }

}
