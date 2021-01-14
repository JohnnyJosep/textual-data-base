using System.Text.Json;

namespace TextualDatabaseApi.Application.Extensions
{
    public static class JsonExtensions
    {
        public static string AsJson(this object obj) => JsonSerializer.Serialize(obj);
    }
}
