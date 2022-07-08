using System.Text.Json;

namespace PipefittersAccounting.UI.Utilities
{
    public static class DebuggingExtensions
    {
        private static JsonSerializerOptions _options = new() { WriteIndented = true };

        public static string ToJson(this object obj) => JsonSerializer.Serialize(obj, _options);
    }
}