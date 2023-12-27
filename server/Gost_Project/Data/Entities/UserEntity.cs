using Gost_Project.Data.Entities.Navigations;

namespace Gost_Project.Data.Entities;

public class UserEntity : BaseEntity
{
    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public UserRoles Role { get; set; }
}
