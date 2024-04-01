using System.Text.Json.Serialization;

namespace GostStorage.Services.Models.Accounts
{
    public class UserAdminEditModel
    {
        [JsonPropertyName("login")]
        public required string Login { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("org_name")]
        public string? OrgName { get; set; }

        [JsonPropertyName("org_branch")]
        public string? OrgBranch { get; set; }

        [JsonPropertyName("org_activity")]
        public string? OrgActivity { get; set; }
    }
}
