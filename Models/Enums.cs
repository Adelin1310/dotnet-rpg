using System.Text.Json.Serialization;

namespace dotnet_rpg
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass{
        Brawler,
        Mage,
        Thief,
        Archer,
        Knight
    }
}