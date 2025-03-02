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

        [Fact]
        public void Params_Property_CanBeSetAndRetrieved()
        {
            // Arrange
            var mockParams1 = new Mock<IReportQueryParams>().Object;
            var mockParams2 = new Mock<IReportQueryParams>().Object;
            var mockItems = new Mock<IPaginatedList>().Object;
            var viewModel = new ItemsPerPageSelectorViewModel(mockParams1, mockItems);

            // Act
            viewModel.Params = mockParams2;

            // Assert
            Assert.Equal(mockParams2, viewModel.Params);
        }

        [Fact]
        public void Items_Property_CanBeSetAndRetrieved()
        {
            // Arrange
            var mockParams = new Mock<IReportQueryParams>().Object;
            var mockItems1 = new Mock<IPaginatedList>(MockBehavior.Strict).Object;
            var mockItems2 = new Mock<IPaginatedList>(MockBehavior.Strict).Object;

            var viewModel = new ItemsPerPageSelectorViewModel(mockParams, mockItems1);

            // Act
            viewModel.Items = mockItems2;

            // Assert
            Assert.NotNull(viewModel.Items); // Confirm it's not null before comparison
            Assert.Same(mockItems2, viewModel.Items); // Use Assert.Same to check reference equality
        }

    }
}
