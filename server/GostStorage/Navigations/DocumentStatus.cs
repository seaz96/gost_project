using System.Text.Json.Serialization;

namespace GostStorage.Navigations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DocumentStatus
{
    Valid,
    Canceled,
    Replaced,
    Inactive
}