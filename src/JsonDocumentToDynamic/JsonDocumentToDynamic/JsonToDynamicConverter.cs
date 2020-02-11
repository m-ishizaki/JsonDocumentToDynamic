using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Rksoftware.JsonDocumentToDynamic
{
    public static class JsonToDynamicConverter
    {
        public static ExpandoObject Parse(JsonElement element) => toExpandoObject(element);

        public static ExpandoObject Parse(JsonDocument document) => toExpandoObject(document.RootElement);

        public static ExpandoObject Parse(string json, JsonDocumentOptions options = default)
        {
            using var document = JsonDocument.Parse(json, options);
            return toExpandoObject(document.RootElement);
        }

        static object propertyValue(JsonElement elm) =>
            elm.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.Number => elm.GetDecimal(),
                JsonValueKind.String => elm.GetString(),
                JsonValueKind.False => false,
                JsonValueKind.True => true,
                JsonValueKind.Array => elm.EnumerateArray().Select(m => propertyValue(m)).ToArray(),
                _ => toExpandoObject(elm),
            };

        static ExpandoObject toExpandoObject(JsonElement elm) =>
            elm.EnumerateObject()
            .Aggregate(
                new ExpandoObject(),
                (exo, prop) => { ((IDictionary<string, object>)exo).Add(prop.Name, propertyValue(prop.Value)); return exo; });
    }
}
