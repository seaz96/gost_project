using System.Text.Json.Serialization;

namespace GostStorage.Navigations;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionType
{
    Create, 
    Update,
    View
}