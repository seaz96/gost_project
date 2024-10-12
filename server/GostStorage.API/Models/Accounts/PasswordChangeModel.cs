using System.Text.Json.Serialization;

namespace GostStorage.API.Models.Accounts
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
