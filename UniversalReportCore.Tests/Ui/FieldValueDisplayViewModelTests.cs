using Xunit;
using Moq;
using UniversalReportCore.Ui.ViewModels;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Tests
{
    public class FieldValueDisplayViewModelTests
    {
        public class TestViewModel : BaseEntityViewModel, IEntityViewModel<int>
        {
            public string Name { get; set; } = "Test Name";
            public decimal Price { get; set; } = 99.99m;
        }

        // TODO: Rewrite tests
        //[Fact]
        //public void Constructor_SetsItemAndPropertyName()
        //{
        //    // Arrange
        //    var item = new TestViewModel();
        //    var propertyName = "Name";

        //    // Act
        //    var viewModel = new FieldValueDisplayViewModel(item, propertyName);

        //    // Assert
        //    Assert.Equal(item, viewModel.Item);
        //    Assert.Equal(propertyName, viewModel.PropertyName);
        //}

        //[Fact]
        //public void GetValue_ReturnsCorrectPropertyValue()
        //{
        //    // Arrange
        //    var item = new TestViewModel { Name = "Sample Name" };
        //    var viewModel = new FieldValueDisplayViewModel(item, "Name");

        //    // Act
        //    var value = viewModel.GetValue();

        //    // Assert
        //    Assert.Equal("Sample Name", value);
        //}

        //[Fact]
        //public void GetValue_ReturnsNull_WhenPropertyDoesNotExist()
        //{
        //    // Arrange
        //    var item = new TestViewModel();
        //    var viewModel = new FieldValueDisplayViewModel(item, "NonExistentProperty");

        //    // Act
        //    var value = viewModel.GetValue();

        //    // Assert
        //    Assert.Null(value);
        //}

        //[Fact]
        //public void GetValue_ReturnsNull_WhenItemIsNull()
        //{
        //    // Arrange
        //    FieldValueDisplayViewModel viewModel = new(null!, "Name");

        //    // Act
        //    var value = viewModel.GetValue();

        //    // Assert
        //    Assert.Null(value);
        //}

        //[Fact]
        //public void GetValue_ReturnsDecimalValue()
        //{
        //    // Arrange
        //    var item = new TestViewModel { Price = 150.75m };
        //    var viewModel = new FieldValueDisplayViewModel(item, "Price");

        //    // Act
        //    var value = viewModel.GetValue();

        //    // Assert
        //    Assert.Equal(150.75m, value);
        //}
    }
}
