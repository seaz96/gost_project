using GostStorage.Entities.Base;
using GostStorage.Navigations;

namespace GostStorage.Entities;

public class UserEntity : BaseEntity
{
    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public UserRoles Role { get; set; }

    public string? OrgName { get; set; }

    public string? OrgBranch { get; set; }

    public string? OrgActivity { get; set; }
}
