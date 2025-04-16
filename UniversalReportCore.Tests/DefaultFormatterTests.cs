using System;
using UniversalReportCore.Ui.ViewModels.FieldFormatting;
using Xunit;

namespace UniversalReportCore.Ui.ViewModels.Tests.FieldFormatting
{
    public class DefaultFormatterTests
    {
        private readonly DefaultFormatter _formatter = new DefaultFormatter();

        [Fact]
        public void CanHandle_AnyValue_ReturnsTrue()
        {
            var value = 42;
            var result = _formatter.CanHandle(value, typeof(int));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_NullValue_ReturnsTrue()
        {
            var result = _formatter.CanHandle(null, typeof(string));
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_AnyType_ReturnsTrue()
        {
            var result = _formatter.CanHandle("test", typeof(string));
            Assert.True(result);
        }

        [Fact]
        public void Format_NullValue_ReturnsEmptyString()
        {
            var result = _formatter.Format(null, typeof(string));
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Format_IntValue_ReturnsString()
        {
            var value = 42;
            var result = _formatter.Format(value, typeof(int));
            Assert.Equal("42", result);
        }

        [Fact]
        public void Format_StringValue_ReturnsString()
        {
            var value = "Hello";
            var result = _formatter.Format(value, typeof(string));
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void Format_DoubleValue_ReturnsString()
        {
            var value = 3.14159;
            var result = _formatter.Format(value, typeof(double));
            Assert.Equal("3.14159", result);
        }
    }
}