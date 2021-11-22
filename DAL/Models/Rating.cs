using System.Text.Json.Serialization;

namespace DAL.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Rating
    {
        All,
        SixPlus,
        TwelvePlus,
        EighteenPlus
    }
}
