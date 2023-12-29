namespace Gost_Project.Data.Models
{
    public class UserEditModel
    {
        public required string Login { get; set; }

        public string? OrgName { get; set; }

        public string? OrgBranch { get; set; }
        
        public string? OrgActivity { get; set; }

        public bool IsAdmin { get; set; }
    }
}
