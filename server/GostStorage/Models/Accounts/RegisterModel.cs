using System.ComponentModel.DataAnnotations;
using GostStorage.Constraints;

namespace GostStorage.Models.Accounts;

public class RegisterModel
{
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,5}$")]
    public required string Login { get; set; }

    [MinLength(LoginModelConstraints.NameMinLength)]
    public required string Name { get; set; }

    [MinLength(LoginModelConstraints.PasswordMinLength)]
    [MaxLength(LoginModelConstraints.PasswordMaxLength)]
    public required string Password { get; set; }

    public string? OrgName { get; set; }

    public string? OrgBranch { get; set; }

    public string? OrgActivity { get; set; }
}