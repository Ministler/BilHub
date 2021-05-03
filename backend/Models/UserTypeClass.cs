using System.Text.Json.Serialization;

namespace backend.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserTypeClass
    {
        Student,
        Instructor
    }
}