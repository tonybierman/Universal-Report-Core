using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.ViewModels;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class ChartMetaViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var chartMeta = new ChartMetaViewModel();

            // Assert
            Assert.NotNull(chartMeta);
            Assert.Equal("bar", chartMeta.ChartType);
            Assert.Null(chartMeta.AxisXLabel);
            Assert.Null(chartMeta.AxisYLabel);
            Assert.Null(chartMeta.TakeTop);
            Assert.Null(chartMeta.PartialName);
            Assert.False(chartMeta.IgnoreZeroes);
            Assert.False(chartMeta.IsHistorical);
        }

        [Fact]
        public void Properties_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var chartMeta = new ChartMetaViewModel
            {
                Title = "Sales Report",
                Subtitle = "Q1 2024",
                AxisXLabel = "Revenue",
                AxisYLabel = "Lorem Ipsum",
                ChartType = "line",
                IgnoreZeroes = true,
                TakeTop = 10,
                DataEndpoint = "/api/chart/data",
                IsHistorical = true,
                PartialName = "_ChartPartial"
            };

            // Assert
            Assert.Equal("Sales Report", chartMeta.Title);
            Assert.Equal("Q1 2024", chartMeta.Subtitle);
            Assert.Equal("Revenue", chartMeta.AxisXLabel);
            Assert.Equal("Lorem Ipsum", chartMeta.AxisYLabel);
            Assert.Equal("line", chartMeta.ChartType);
            Assert.True(chartMeta.IgnoreZeroes);
            Assert.Equal(10, chartMeta.TakeTop);
            Assert.Equal("/api/chart/data", chartMeta.DataEndpoint);
            Assert.True(chartMeta.IsHistorical);
            Assert.Equal("_ChartPartial", chartMeta.PartialName);
        }
    }

}
