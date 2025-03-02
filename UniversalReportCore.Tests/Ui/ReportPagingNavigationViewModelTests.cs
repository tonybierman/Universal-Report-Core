using Xunit;
using Moq;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.Ui.ViewModels;

namespace UniversalReportCore.Tests
{
    public class ReportPagingNavigationViewModelTests
    {
        [Fact]
        public void Can_Initialize_With_Valid_Properties()
        {
            // Arrange
            var mockParams = new Mock<IReportQueryParams>().Object;
            var mockItems = new Mock<IPaginatedList>().Object;

            var viewModel = new ReportPagingNavigationViewModel
            {
                CurrentSort = "NameAsc",
                Params = mockParams,
                Items = mockItems
            };

            // Assert
            Assert.Equal("NameAsc", viewModel.CurrentSort);
            Assert.Equal(mockParams, viewModel.Params);
            Assert.Same(mockItems, viewModel.Items);
        }

        [Fact]
        public void Can_Set_And_Get_CurrentSort()
        {
            // Arrange
            var viewModel = new ReportPagingNavigationViewModel();

            // Act
            viewModel.CurrentSort = "DateDesc";

            // Assert
            Assert.Equal("DateDesc", viewModel.CurrentSort);
        }

        [Fact]
        public void Can_Set_And_Get_Params()
        {
            // Arrange
            var mockParams = new Mock<IReportQueryParams>().Object;
            var viewModel = new ReportPagingNavigationViewModel();

            // Act
            viewModel.Params = mockParams;

            // Assert
            Assert.Equal(mockParams, viewModel.Params);
        }

        [Fact]
        public void Can_Set_And_Get_Items()
        {
            // Arrange
            var mockItems = new Mock<IPaginatedList>().Object;
            var viewModel = new ReportPagingNavigationViewModel();

            // Act
            viewModel.Items = mockItems;

            // Assert
            Assert.Same(mockItems, viewModel.Items);
        }
    }
}
