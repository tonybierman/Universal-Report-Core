using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace UniversalReportCore.Ui.Helpers
{
    public static class TempDataHelper
    {
        /// <summary>
        /// Determines if aggregates need to be recalculated by tracking changes in slug and cohort IDs.
        /// </summary>
        public static bool ShouldRecalculateAggregates(ITempDataDictionary tempData, string slug, int[]? cohortIds)
        {
            var parametersSnapshot = new { Slug = slug, CohortIds = cohortIds };
            var newJson = JsonSerializer.Serialize(parametersSnapshot);

            var existingJson = tempData["QueryParameters"] as string;
            bool hasChanged = existingJson == null || existingJson != newJson;

            tempData["QueryParameters"] = newJson;
            return hasChanged;
        }

        /// <summary>
        /// Serializes aggregate data to TempData for persistence across requests.
        /// </summary>
        public static void SerializeAggregatesToTempData(ITempDataDictionary tempData, Dictionary<string, dynamic>? aggregates)
        {
            if (aggregates != null)
            {
                tempData["Aggregates"] = JsonSerializer.Serialize(aggregates);
            }
        }

        /// <summary>
        /// Deserializes aggregate data from TempData, ensuring proper type conversion.
        /// </summary>
        public static Dictionary<string, dynamic>? DeserializeAggregatesFromTempData(ITempDataDictionary tempData)
        {
            if (tempData.TryGetValue("Aggregates", out var json) && json is string jsonString)
            {
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
                return dictionary?.ToDictionary(
                    kvp => kvp.Key,
                    kvp => ConvertJsonElement(kvp.Value)
                );
            }
            return null;
        }

        /// <summary>
        /// Converts JsonElement values to appropriate C# types.
        /// </summary>
        private static object ConvertJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Number => element.TryGetDouble(out double d) ? d : element.GetDecimal(),
                JsonValueKind.String => element.GetString()!,
                JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
                JsonValueKind.Null => null!,
                _ => element // Leave as is for unsupported cases
            };
        }
    }
}
