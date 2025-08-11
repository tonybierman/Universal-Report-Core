using System;
using UniversalReportCore;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class ReportColumnDefinitionTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var column = new ReportColumnDefinition();

            Assert.Equal(column.DisplayName, string.Empty);
            Assert.Equal(column.PropertyName, string.Empty);
            Assert.Equal(column.ViewModelName, string.Empty);
            Assert.False(column.IsDisplayKey);
            Assert.False(column.IsSortable);
            Assert.Null(column.DefaultSort);
            Assert.Null(column.CssClass);
            Assert.False(column.IsSortDescending);
            Assert.Null(column.RenderPartial);
            Assert.False(column.HideInPortrait);
            Assert.Equal(AggregationType.None, column.Aggregation);
        }

        [Fact]
        public void Properties_ShouldSetAndGetValuesCorrectly()
        {
            var column = new ReportColumnDefinition
            {
                DisplayName = "Test Column",
                PropertyName = "TestProperty",
                ViewModelName = "TestViewModel",
                IsDisplayKey = true,
                IsSortable = true,
                DefaultSort = "asc",
                CssClass = "test-class",
                IsSortDescending = true,
                RenderPartial = "TestPartial",
                HideInPortrait = true,
                Aggregation = AggregationType.Sum
            };

            Assert.Equal("Test Column", column.DisplayName);
            Assert.Equal("TestProperty", column.PropertyName);
            Assert.Equal("TestViewModel", column.ViewModelName);
            Assert.True(column.IsDisplayKey);
            Assert.True(column.IsSortable);
            Assert.Equal("asc", column.DefaultSort);
            Assert.Equal("test-class", column.CssClass);
            Assert.True(column.IsSortDescending);
            Assert.Equal("TestPartial", column.RenderPartial);
            Assert.True(column.HideInPortrait);
            Assert.Equal(AggregationType.Sum, column.Aggregation);
        }

        [Fact]
        public void NullableProperties_ShouldAllowNullValues()
        {
            var column = new ReportColumnDefinition
            {
                DisplayName = null,
                PropertyName = null,
                ViewModelName = null,
                DefaultSort = null,
                CssClass = null,
                RenderPartial = null
            };

            Assert.Null(column.DisplayName);
            Assert.Null(column.PropertyName);
            Assert.Null(column.ViewModelName);
            Assert.Null(column.DefaultSort);
            Assert.Null(column.CssClass);
            Assert.Null(column.RenderPartial);
        }

        [Fact]
        public void AggregationType_ShouldSupportAllEnumValues()
        {
            var column = new ReportColumnDefinition();

            foreach (AggregationType value in Enum.GetValues(typeof(AggregationType)))
            {
                column.Aggregation = value;
                Assert.Equal(value, column.Aggregation);
            }
        }
    }
}