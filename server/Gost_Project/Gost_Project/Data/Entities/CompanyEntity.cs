using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class CompanyEntity : BaseEntity
{
    public required string Login { get; set; }

    public required string Password { get; set; }
    
    public required string Name { get; set; }
}