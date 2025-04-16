using System;
using System.Reflection;
using Moq;
using UniversalReportCore.Ui.ViewModels;
using UniversalReportCore.Ui.ViewModels.FieldFormatting;
using UniversalReportCore.ViewModels;
using Xunit;

namespace UniversalReportCore.Ui.ViewModels.Tests
{
    public class FieldValueDisplayViewModelTests
    {
        private readonly Mock<IEntityViewModel<int>> _mockItem;
        private readonly Mock<IReportColumnDefinition> _mockColumnDefinition;
        private readonly FieldValueDisplayViewModel _viewModel;

        public FieldValueDisplayViewModelTests()
        {
            _mockItem = new Mock<IEntityViewModel<int>>();
            _mockColumnDefinition = new Mock<IReportColumnDefinition>();
            _mockColumnDefinition.Setup(c => c.PropertyName).Returns("TestProperty");
            _mockColumnDefinition.Setup(c => c.ViewModelName).Returns((string)null);
            _viewModel = new FieldValueDisplayViewModel(_mockItem.Object, _mockColumnDefinition.Object);
        }

        [Fact]
        public void Constructor_SetsItemCorrectly()
        {
            Assert.Equal(_mockItem.Object, _viewModel.Item);
        }

        [Fact]
        public void Constructor_SetsColumnDefinitionCorrectly()
        {
            Assert.Equal(_mockColumnDefinition.Object, _viewModel.ColumnDefinition);
        }

        [Fact]
        public void Constructor_SetsPropertyName_FromPropertyName_WhenViewModelNameIsNull()
        {
            Assert.Equal("TestProperty", _viewModel.PropertyName);
        }

        [Fact]
        public void Constructor_SetsPropertyName_FromViewModelName_WhenNotNull()
        {
            _mockColumnDefinition.Setup(c => c.ViewModelName).Returns("CustomName");
            var viewModel = new FieldValueDisplayViewModel(_mockItem.Object, _mockColumnDefinition.Object);
            Assert.Equal("CustomName", viewModel.PropertyName);
        }

        [Fact]
        public void GetValue_ReturnsPropertyValue_WhenPropertyExists()
        {
            var testObject = new TestClass { TestProperty = 42 };
            _viewModel.Item = testObject;

            var result = _viewModel.GetValue();

            Assert.Equal(42, result);
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenPropertyDoesNotExist()
        {
            var testObject = new TestClass();
            _viewModel.Item = testObject;
            _viewModel.PropertyName = "NonExistentProperty";

            var result = _viewModel.GetValue();

            Assert.Null(result);
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenItemIsNull()
        {
            _viewModel.Item = null;

            var result = _viewModel.GetValue();

            Assert.Null(result);
        }

        [Fact]
        public void FormatFieldValue_IntValue_ReturnsString()
        {
            var result = _viewModel.FormatFieldValue(42, typeof(int));
            Assert.Equal("42", result);
        }

        [Fact]
        public void FormatFieldValue_LongValue_ReturnsString()
        {
            var result = _viewModel.FormatFieldValue(123456789L, typeof(long));
            Assert.Equal("123456789", result);
        }

        [Fact]
        public void FormatFieldValue_NullIntType_ReturnsZero()
        {
            var result = _viewModel.FormatFieldValue(null, typeof(int));
            Assert.Equal("0", result);
        }

        [Fact]
        public void FormatFieldValue_DoubleValue_ReturnsFormattedDecimal()
        {
            var result = _viewModel.FormatFieldValue(3.14159, typeof(double));
            Assert.Equal("3.14", result);
        }

        [Fact]
        public void FormatFieldValue_FloatValue_ReturnsFormattedDecimal()
        {
            var result = _viewModel.FormatFieldValue(2.718f, typeof(float));
            Assert.Equal("2.72", result);
        }

        [Fact]
        public void FormatFieldValue_DecimalValue_ReturnsFormattedDecimal()
        {
            var result = _viewModel.FormatFieldValue(123.456m, typeof(decimal));
            Assert.Equal("123.46", result);
        }

        [Fact]
        public void FormatFieldValue_NullDecimalType_ReturnsFormattedZero()
        {
            var result = _viewModel.FormatFieldValue(null, typeof(decimal));
            Assert.Equal("0.00", result);
        }

        [Fact]
        public void FormatFieldValue_StringValue_ReturnsString()
        {
            var result = _viewModel.FormatFieldValue("Hello", typeof(string));
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void FormatFieldValue_NullStringType_ReturnsEmptyString()
        {
            var result = _viewModel.FormatFieldValue(null, typeof(string));
            Assert.Equal(string.Empty, result);
        }

        // Helper class for testing property access
        private class TestClass
        {
            public int TestProperty { get; set; }
        }
    }
}