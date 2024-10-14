using System.Text.Json.Serialization;

namespace GostStorage.Models.Accounts;

public class AuthResponseModel
{
    [JsonPropertyName("token")] public required string Token { get; set; }
}