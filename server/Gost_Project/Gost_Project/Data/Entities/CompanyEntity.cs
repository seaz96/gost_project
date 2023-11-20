using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Entities;

public class CompanyEntity : BaseEntity
{
    [Required] 
    public string Login { get; set; }

    [Required] 
    public string Password { get; set; }
    
    public string Name { get; set; }
}