using System.Text.Json.Serialization;

namespace GostStorage.Navigations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AdoptionLevel
{
    International,
    Foreign,
    Regional,
    Organizational,
    National,
    Interstate,
}