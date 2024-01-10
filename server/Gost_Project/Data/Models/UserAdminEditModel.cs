using System.Text.Json.Serialization;

namespace Gost_Project.Data.Models
{
    public class UserAdminEditModel
    {
        [JsonPropertyName("login")]
        public required string Login { get; set; }

        [JsonPropertyName("org_name")]
        public string? OrgName { get; set; }

        [JsonPropertyName("org_branch")]
        public string? OrgBranch { get; set; }

        [JsonPropertyName("org_activity")]
        public string? OrgActivity { get; set; }
    }
}
