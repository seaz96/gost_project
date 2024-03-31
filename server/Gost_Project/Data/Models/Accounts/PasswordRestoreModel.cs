using System.Text.Json.Serialization;

namespace Gost_Project.Data.Models.Accounts
{
    public class PasswordRestoreModel
    {
        [JsonPropertyName("login")]
        public required string Login { get; set; }

        [JsonPropertyName("new_password")]
        public required string NewPassword { get; set; }
    }
}
