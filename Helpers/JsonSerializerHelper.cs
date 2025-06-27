using System.Text.Json;

namespace Helpers
{
    public static class JsonSerializerHelper
    {
        private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static string Serialize<T>(T obj, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(obj, options ?? DefaultOptions);
        }

        public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, options ?? DefaultOptions);
            }
            catch (JsonException)
            {
                // Aquí puedes loggear o manejar error si quieres
                return default;
            }
        }
    }
}
