using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class UserEntity : BaseEntity
{
    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public Role Role { get; set; }

    public string? Name { get; set; }
    
    public long CompanyId { get; set; }
}

public enum Role
{
    User,
    Admin
}