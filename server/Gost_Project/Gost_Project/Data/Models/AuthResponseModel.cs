using System.Text.Json.Serialization;

namespace Gost_Project.Data.Models
{
    public class AuthResponseModel
    {
        [JsonPropertyName("token")]
        public required string Token { get; set; }
    }
}
