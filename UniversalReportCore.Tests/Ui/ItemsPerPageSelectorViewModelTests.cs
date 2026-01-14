using Xunit;
using Moq;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.Ui.ViewModels;

namespace UniversalReportCore.Tests
{
    public class ItemsPerPageSelectorViewModelTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var mockParams = new Mock<IReportQueryParams>().Object;
            var mockItems = new Mock<IPaginatedList>().Object;

            // Act
            var viewModel = new ItemsPerPageSelectorViewModel(mockParams, mockItems);

            // Assert
            Assert.Equal(mockParams, viewModel.Params);
            Assert.Same(mockItems, viewModel.Items);
        }
    }
}
