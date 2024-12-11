using System.ComponentModel.DataAnnotations;
using GostStorage.Constraints;

namespace GostStorage.Models.Accounts;

public class LoginModel
{
    public required string Login { get; set; }

    [MinLength(LoginModelConstraints.PasswordMinLength)]
    [MaxLength(LoginModelConstraints.PasswordMaxLength)]
    public required string Password { get; set; }
}