using System;
using UniversalReportCore.Ui.ViewModels.FieldFormatting;
using Xunit;

namespace UniversalReportCore.Ui.ViewModels.Tests.FieldFormatting
{
    public class IntegerFormatterTests
    {
        private readonly IntegerFormatter _formatter = new IntegerFormatter();

        [Fact]
        public void CanHandle_IntValue_ReturnsTrue()
        {
            var value = 42;
            var result = _formatter.CanHandle(value, typeof(int));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_LongValue_ReturnsTrue()
        {
            var value = 123456789L;
            var result = _formatter.CanHandle(value, typeof(long));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithIntType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(int));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithNullableIntType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(int?));
            Assert.True(result);
        }

        [Fact]
       

 public void CanHandle_NullWithLongType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(long));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullWithNullableLongType_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(long?));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NonIntegerValue_ReturnsFalse()
        {
            var value = 3.14;
            var result = _formatter.CanHandle(value, typeof(double));
            Assert.False(result);
        }

        [Fact]
        public void CanHandle_NullWithNonIntegerType_ReturnsFalse()
        {
            var result = _formatter.CanHandle(null, typeof(string));
            Assert.False(result);
        }

        [Fact]
        public void Format_IntValue_ReturnsCorrectString()
        {
            var value = 42;
            var result = _formatter.Format(value, typeof(int));
            Assert.Equal("42", result);
        }

        [Fact]
        public void Format_LongValue_ReturnsCorrectString()
        {
            var value = 123456789L;
            var result = _formatter.Format(value, typeof(long));
            Assert.Equal("123456789", result);
        }

        [Fact]
        public void Format_NullValue_ReturnsZero()
        {
            var result = _formatter.Format(null, typeof(int));
            Assert.Equal("0", result);
        }
    }
}