using System.Text.Json.Serialization;

namespace DAL.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Platform
    {
        PersonalComputer,
        Mobile,
        PlayStation,
        Xbox,
        Nintendo
    }
}
