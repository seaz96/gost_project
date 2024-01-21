using Gost_Project.Data.Models.Attributes;

namespace Gost_Project.Data.Models
{
    public class UserSelfEditModel
    {
        public string? Name { get; set; }
        
        public string? OrgName { get; set; }

        public string? OrgBranch { get; set; }
        
        public string? OrgActivity { get; set; }
    }
}
