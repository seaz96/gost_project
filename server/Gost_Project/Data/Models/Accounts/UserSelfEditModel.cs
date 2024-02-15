using System.Text.Json.Serialization;
using Gost_Project.Data.Models.Attributes;

namespace Gost_Project.Data.Models
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
