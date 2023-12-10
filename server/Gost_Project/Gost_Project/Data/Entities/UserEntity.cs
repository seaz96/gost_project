using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class UserEntity : BaseEntity
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
    
    public Role Role { get; set; }

    public string? Name { get; set; }
    
    public long CompanyId { get; set; }
}

public enum Role
{
    User,
    Admin
}