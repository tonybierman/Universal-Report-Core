using System;
using UniversalReportCore.Ui.ViewModels.FieldFormatting;
using Xunit;

namespace UniversalReportCore.Ui.ViewModels.Tests.FieldFormatting
{
    public class DecimalFormatterTests
    {
        private readonly DecimalFormatter _formatter = new DecimalFormatter();

        [Fact]
        public void CanHandle_DoubleValue_ReturnsTrue()
        {
            var value = 3.14159;
            var result = _formatter.CanHandle(value, typeof(double));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_FloatValue_ReturnsTrue()
        {
            var value = 2.718f;
            var result = _formatter.CanHandle(value, typeof(float));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_DecimalValue_ReturnsTrue()
        {
            var value = 123.456m;
            var result = _formatter.CanHandle(value, typeof(decimal));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithDoubleType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(double));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithNullableDoubleType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(double?));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithFloatType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(float));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithNullableFloatType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(float?));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithDecimalType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(decimal));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithNullableDecimalType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(decimal?));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NonDecimalValue_ReturnsFalse()
        {
            var value = 42;
            var result = _formatter.CanHandle(value, typeof(int));
            Assert.False(result);
        }

        [Fact]
        public void CanHandle_NullWithNonDecimalType_ReturnsFalse()
        {
            var result = _formatter.CanHandle(null, typeof(string));
            Assert.False(result);
        }

        [Fact]
        public void Format_DoubleValue_ReturnsFormattedString()
        {
            var value = 3.14159;
            var result = _formatter.Format(value, typeof(double));
            Assert.Equal("3.14", result);
        }

        [Fact]
        public void Format_FloatValue_ReturnsFormattedString()
        {
            var value = 2.718f;
            var result = _formatter.Format(value, typeof(float));
            Assert.Equal("2.72", result);
        }

        [Fact]
        public void Format_DecimalValue_ReturnsFormattedString()
        {
            var value = 123.456m;
            var result = _formatter.Format(value, typeof(decimal));
            Assert.Equal("123.46", result);
        }

        [Fact]
        public void Format_NullValue_ReturnsFormattedZero()
        {
            var result = _formatter.Format(null, typeof(double));
            Assert.Equal("0.00", result);
        }
    }
}