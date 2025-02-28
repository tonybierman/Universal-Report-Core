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
                Mock.Of<IReportColumnDefinition>(c => c.IsDisplayKey == true),
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
        public void GetReportColumns_ShouldThrowException_WhenNoColumnHasIsDisplayKey()
        {
            // Arrange
            var slug = "test-report";
            var columnsWithoutDisplayKey = new List<IReportColumnDefinition>
            {
                Mock.Of<IReportColumnDefinition>(c => c.IsDisplayKey == false),
                Mock.Of<IReportColumnDefinition>(c => c.IsDisplayKey == false)
            };

            var mockProvider = new Mock<IReportColumnProvider>();
            mockProvider.Setup(p => p.Slug).Returns(slug);
            mockProvider.Setup(p => p.GetColumns()).Returns(columnsWithoutDisplayKey);

            var factory = new ReportColumnFactory(new List<IReportColumnProvider> { mockProvider.Object });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => factory.GetReportColumns(slug));
            Assert.Equal("Expected exactly one column with IsDisplayKey set to true, but found 0.", exception.Message);
        }

        [Fact]
        public void GetReportColumns_ShouldThrowException_WhenMultipleColumnsHaveIsDisplayKey()
        {
            // Arrange
            var slug = "test-report";
            var columnsWithMultipleDisplayKeys = new List<IReportColumnDefinition>
            {
                Mock.Of<IReportColumnDefinition>(c => c.IsDisplayKey == true),
                Mock.Of<IReportColumnDefinition>(c => c.IsDisplayKey == true)
            };

            var mockProvider = new Mock<IReportColumnProvider>();
            mockProvider.Setup(p => p.Slug).Returns(slug);
            mockProvider.Setup(p => p.GetColumns()).Returns(columnsWithMultipleDisplayKeys);

            var factory = new ReportColumnFactory(new List<IReportColumnProvider> { mockProvider.Object });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => factory.GetReportColumns(slug));
            Assert.Equal("Expected exactly one column with IsDisplayKey set to true, but found 2.", exception.Message);
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
