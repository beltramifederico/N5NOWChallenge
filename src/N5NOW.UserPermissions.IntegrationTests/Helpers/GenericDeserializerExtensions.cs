using System.Text.Json;
using System.Text.Json.Serialization;

namespace N5NOW.UserPermissions.IntegrationTests.Helpers
{
    public static class GenericDeserializerExtensions
    {
        public static T DeserializeCustom<T>(this HttpResponseMessage response)
        {
            string json = response.Content.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            var result = JsonSerializer.Deserialize<T>(json, options);

            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }
    }
}
