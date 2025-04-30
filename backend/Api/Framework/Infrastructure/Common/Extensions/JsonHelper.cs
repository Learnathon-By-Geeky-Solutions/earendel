using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace TalentMesh.Framework.Infrastructure.Common
{
    [ExcludeFromCodeCoverage]
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static T? Deserialize<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, Options);
            }
            catch (Exception)
            {
                // The caller handles a null return value.
                return default;
            }
        }

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, Options);
        }
    }
}
