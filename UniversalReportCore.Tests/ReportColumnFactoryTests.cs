using Moq;

namespace UniversalReportCore.Tests
{
    public class ReportColumnFactoryTests
    {
        [Fact]
        public void GetReportColumns_ShouldReturnColumns_WhenProviderExists()
        {
            // Arrange
            var slug = "test-report";
            var expectedColumns = new List<IReportColumnDefinition>
            {
                Mock.Of<IReportColumnDefinition>(),
                Mock.Of<IReportColumnDefinition>()
            };

            var mockProvider = new Mock<IReportColumnProvider>();
            mockProvider.Setup(p => p.Slug).Returns(slug);
            mockProvider.Setup(p => p.GetColumns()).Returns(expectedColumns);

            var factory = new ReportColumnFactory(new List<IReportColumnProvider> { mockProvider.Object });

            // Act
            var result = factory.GetReportColumns(slug);

            // Assert
            Assert.Equal(expectedColumns, result);
        }

        [Fact]
        public void GetReportColumns_ShouldThrowInvalidOperationException_WhenProviderDoesNotExist()
        {
            // Arrange
            var factory = new ReportColumnFactory(new List<IReportColumnProvider>()); // Empty provider list
            var slug = "nonexistent-report";

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => factory.GetReportColumns(slug));
            Assert.Equal($"Unsupported report type: {slug}", exception.Message);
        }
    }
}
