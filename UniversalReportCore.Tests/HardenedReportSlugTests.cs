using Moq;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportCore.PageMetadata;

namespace UniversalReportCore.Tests
{
    public class HardenedReportSlugTests
    {
        [Fact]
        public void Validate_ShouldReturnTrue_WhenSlugExistsInPageMetaFactory()
        {
            // Arrange
            var validSlug = "valid-report";
            var expectedCategory = "ValidCategory";

            var mockProvider = new Mock<IPageMetaProvider>();
            mockProvider.Setup(p => p.Slug).Returns(validSlug);
            mockProvider.Setup(p => p.CategorySlug).Returns(expectedCategory);

            var mockPageMetaFactory = new Mock<IPageMetaFactory>();
            mockPageMetaFactory.Setup(f => f.Providers)
                .Returns(new List<IPageMetaProvider> { mockProvider.Object });

            var hardenedSlug = new HardenedReportSlug(validSlug);

            // Act
            var result = hardenedSlug.Validate(mockPageMetaFactory.Object);

            // Assert
            Assert.True(result);
            Assert.True(hardenedSlug.IsValid);
            Assert.Equal(expectedCategory, hardenedSlug.ReportType);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenSlugDoesNotExistInPageMetaFactory()
        {
            // Arrange
            var invalidSlug = "invalid-report";

            var mockPageMetaFactory = new Mock<IPageMetaFactory>();
            mockPageMetaFactory.Setup(f => f.Providers)
                .Returns(new List<IPageMetaProvider>()); // Empty list, no matching slug

            var hardenedSlug = new HardenedReportSlug(invalidSlug);

            // Act
            var result = hardenedSlug.Validate(mockPageMetaFactory.Object);

            // Assert
            Assert.False(result);
            Assert.False(hardenedSlug.IsValid);
            Assert.Null(hardenedSlug.ReportType);
        }

        [Fact]
        public void Validate_ShouldSetReportTypeToNull_WhenSlugIsNotFound()
        {
            // Arrange
            var nonExistentSlug = "non-existent";

            var mockProvider = new Mock<IPageMetaProvider>();
            mockProvider.Setup(p => p.Slug).Returns("some-other-slug");

            var mockPageMetaFactory = new Mock<IPageMetaFactory>();
            mockPageMetaFactory.Setup(f => f.Providers)
                .Returns(new List<IPageMetaProvider> { mockProvider.Object });

            var hardenedSlug = new HardenedReportSlug(nonExistentSlug);

            // Act
            var result = hardenedSlug.Validate(mockPageMetaFactory.Object);

            // Assert
            Assert.False(result);
            Assert.Null(hardenedSlug.ReportType);
        }
    }
}
