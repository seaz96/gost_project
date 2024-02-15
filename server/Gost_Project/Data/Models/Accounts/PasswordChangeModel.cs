using System.Text.Json.Serialization;

namespace Gost_Project.Data.Models
{
    public class PasswordChangeModel
    {
        [JsonPropertyName("login")]
        public required string Login { get; set; }

        [JsonPropertyName("new_password")]
        public required string NewPassword { get; set; }
        
        [JsonPropertyName("old_password")]
        public required string OldPassword { get; set; }
    }
}
