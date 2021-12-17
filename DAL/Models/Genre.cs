using System.Text.Json.Serialization;

namespace DAL.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Genre
    {
        All,
        Shooter,
        Racing,
        Casual,
        Fighting,
        Action
    }
}