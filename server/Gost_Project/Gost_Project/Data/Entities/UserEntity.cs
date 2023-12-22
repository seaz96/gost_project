namespace Gost_Project.Data.Entities;

public class UserEntity : BaseEntity
{
    public string? Login { get; set; }

    public string? Password { get; set; }

    public Role Role { get; set; }

    public string? Name { get; set; }
}

public enum Role
{
    User,
    Admin
}