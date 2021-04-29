using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SemesterType
    {
        Spring,
        Summer,
        Fall
    }
}