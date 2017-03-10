using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Extensions
{
    public static class JsonUnmappedDataExtensions
    {
        public static T GetUnmappedDataField<T>(this IDictionary<string, JToken> unmappedData, string fieldName)
        {
            // If field is not in the unmapped data, or if a non-nullable type is null, return default for the type
            if (!unmappedData.ContainsKey(fieldName) ||
                (unmappedData[fieldName].Type == JTokenType.Null && (typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) == null)))
            {
                return default(T);
            }

            // Return the value of the desired type
            return unmappedData[fieldName].Value<T>();
        }
    }
}
