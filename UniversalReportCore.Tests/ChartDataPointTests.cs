using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using UniversalReportCore.Data;

namespace UniversalReportCore.Tests
{
    public class ChartDataPointTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Act
            var chartDataPoint = new ChartDataPoint();

            // Assert
            Assert.NotNull(chartDataPoint.Values);
            Assert.Empty(chartDataPoint.Values);
            Assert.Equal("default", chartDataPoint.DefaultKey);
        }

        [Fact]
        public void Value_ShouldSetAndGetFromDefaultKey()
        {
            // Arrange
            var chartDataPoint = new ChartDataPoint();

            // Act
            chartDataPoint.Value = 10.5;

            // Assert
            Assert.True(chartDataPoint.Values.ContainsKey("default"));
            Assert.Equal(10.5, chartDataPoint.Values["default"]);
            Assert.Equal(10.5, chartDataPoint.Value);
        }

        [Fact]
        public void Value_ShouldReturnZero_WhenDefaultKeyIsMissing()
        {
            // Arrange
            var chartDataPoint = new ChartDataPoint();

            // Act & Assert
            Assert.Equal(0, chartDataPoint.Value);
        }

        [Fact]
        public void DefaultKey_ShouldAffectValueRetrieval()
        {
            // Arrange
            var chartDataPoint = new ChartDataPoint { DefaultKey = "CustomKey" };
            chartDataPoint.Values["CustomKey"] = 42.7;

            // Act & Assert
            Assert.Equal(42.7, chartDataPoint.Value);
        }

        [Fact]
        public void DefaultKey_ShouldReturnZero_WhenKeyIsNotInDictionary()
        {
            // Arrange
            var chartDataPoint = new ChartDataPoint { DefaultKey = "NonExistentKey" };

            // Act & Assert
            Assert.Equal(0, chartDataPoint.Value);
        }

        [Fact]
        public void SettingValue_ShouldUpdateDefaultKeyEntry()
        {
            // Arrange
            var chartDataPoint = new ChartDataPoint();

            // Act
            chartDataPoint.Value = 99.9;

            // Assert
            Assert.Equal(99.9, chartDataPoint.Values["default"]);
        }

        [Fact]
        public void Values_ShouldSupportMultipleDatasets()
        {
            // Arrange
            var chartDataPoint = new ChartDataPoint();
            chartDataPoint.Values["Dataset1"] = 10.5;
            chartDataPoint.Values["Dataset2"] = 20.5;

            // Act & Assert
            Assert.Equal(10.5, chartDataPoint.Values["Dataset1"]);
            Assert.Equal(20.5, chartDataPoint.Values["Dataset2"]);
        }
    }

}
