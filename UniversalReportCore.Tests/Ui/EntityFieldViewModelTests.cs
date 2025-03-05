using Xunit;
using Moq;
using UniversalReportCore.ViewModels;
using UniversalReportCore.Ui.ViewModels;

namespace UniversalReportCore.Tests
{
    public class EntityFieldViewModelTests
    {
        [Fact]
        public void Constructor_SetsParentAndColumn()
        {
            // Arrange
            var mockParent = new Mock<IEntityViewModel<int>>();
            var mockColumn = new Mock<IReportColumnDefinition>();

            // Act
            var viewModel = new EntityFieldViewModel(mockParent.Object, mockColumn.Object, null);

            // Assert
            Assert.Equal(mockParent.Object, viewModel.Parent);
            Assert.Equal(mockColumn.Object, viewModel.Column);
        }

        [Fact]
        public void Slug_CanBeSetAndRetrieved()
        {
            // Arrange
            var mockParent = new Mock<IEntityViewModel<int>>();
            var mockColumn = new Mock<IReportColumnDefinition>();
            var viewModel = new EntityFieldViewModel(mockParent.Object, mockColumn.Object, null);

            // Act
            viewModel.Slug = "test-slug";

            // Assert
            Assert.Equal("test-slug", viewModel.Slug);
        }
    }
}
