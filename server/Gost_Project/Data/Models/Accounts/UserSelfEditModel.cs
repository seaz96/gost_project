using System.Text.Json.Serialization;

namespace Gost_Project.Data.Models.Accounts
{
    public class UserSelfEditModel
    {
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
