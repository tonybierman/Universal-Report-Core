using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace UniversalReportCore.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        [return: NotNull]
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json), "JSON string cannot be null or empty");

            T result = JsonSerializer.Deserialize<T>(json, _options)
                ?? throw new JsonException($"Deserialization of type {typeof(T).Name} resulted in null");

            return result;
        }

        public static string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }
    }
}
