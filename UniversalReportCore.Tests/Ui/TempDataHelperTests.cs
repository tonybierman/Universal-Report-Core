using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using UniversalReportCore.Ui.Helpers;

namespace UniversalReportCore.Tests
{
    public class TempDataHelperTests
    {
        [Fact]
        public void ShouldRecalculateAggregates_ReturnsTrue_WhenTempDataIsEmpty()
        {
            var tempData = new Mock<ITempDataDictionary>();
            tempData.Setup(t => t["QueryParameters"]).Returns(null);

            var result = TempDataHelper.ShouldRecalculateAggregates(tempData.Object, "test-slug", new[] { 1, 2 }, new[] { "test-filter" });

            Assert.True(result);
            tempData.VerifySet(t => t["QueryParameters"] = It.IsAny<string>(), Times.Once);
        }

        [Fact]
        public void ShouldRecalculateAggregates_ReturnsFalse_WhenParametersUnchanged()
        {
            var snapshot = JsonSerializer.Serialize(new { Slug = "test-slug", CohortIds = new[] { 1, 2 }, FilterKeys = new[] { "test-filter" } });
            var tempData = new Mock<ITempDataDictionary>();
            tempData.Setup(t => t["QueryParameters"]).Returns(snapshot);

            var result = TempDataHelper.ShouldRecalculateAggregates(tempData.Object, "test-slug", new[] { 1, 2 } , new[] { "test-filter" });

            Assert.False(result);
            tempData.VerifySet(t => t["QueryParameters"] = snapshot, Times.Once);
        }

        [Fact]
        public void ShouldRecalculateAggregates_ReturnsTrue_WhenParametersChanged()
        {
            var snapshot = JsonSerializer.Serialize(new { Slug = "old-slug", CohortIds = new[] { 1 } });
            var tempData = new Mock<ITempDataDictionary>();
            tempData.Setup(t => t["QueryParameters"]).Returns(snapshot);

            var result = TempDataHelper.ShouldRecalculateAggregates(tempData.Object, "new-slug", new[] { 1, 2 }, new[] { "test-filter" });

            Assert.True(result);
            tempData.VerifySet(t => t["QueryParameters"] = It.IsAny<string>(), Times.Once);
        }

        [Fact]
        public void SerializeAggregatesToTempData_SetsSerializedJson()
        {
            var tempData = new Mock<ITempDataDictionary>();
            var aggregates = new Dictionary<string, dynamic>
            {
                { "Total", 123.45 },
                { "Count", 10 }
            };

            TempDataHelper.SerializeAggregatesToTempData(tempData.Object, aggregates);

            tempData.VerifySet(t => t["Aggregates"] = JsonSerializer.Serialize(aggregates), Times.Once);
        }

        [Fact]
        public void DeserializeAggregatesFromTempData_ReturnsCorrectDictionary()
        {
            var originalAggregates = new Dictionary<string, dynamic>
    {
        { "Total", 123.45 },
        { "Count", 10 },
        { "Label", "Test" },
        { "IsActive", true }
    };
            var jsonString = JsonSerializer.Serialize(originalAggregates);
            object? tempDataValue = jsonString;

            var tempData = new Mock<ITempDataDictionary>();
            tempData.Setup(t => t.TryGetValue("Aggregates", out tempDataValue)).Returns(true);

            var result = TempDataHelper.DeserializeAggregatesFromTempData(tempData.Object);

            Assert.NotNull(result);
            Assert.Equal(123.45, result!["Total"]);
            Assert.Equal(10, result["Count"]);
            Assert.Equal("Test", result["Label"]);
            Assert.True(result["IsActive"]);
        }


        [Fact]
        public void DeserializeAggregatesFromTempData_ReturnsNull_WhenNoAggregatesInTempData()
        {
            var tempData = new Mock<ITempDataDictionary>();
            tempData.Setup(t => t.TryGetValue("Aggregates", out It.Ref<object>.IsAny)).Returns(false);

            var result = TempDataHelper.DeserializeAggregatesFromTempData(tempData.Object);

            Assert.Null(result);
        }
    }
}
