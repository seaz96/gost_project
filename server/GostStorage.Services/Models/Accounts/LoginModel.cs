using System.ComponentModel.DataAnnotations;
using GostStorage.Services.Constraints;

namespace GostStorage.Services.Models.Accounts
{
    public class LoginModel
    {
        public required string Login {  get; set; }

        [MinLength(LoginModelConstraints.PASSWORD_MIN_LENGTH)]
        public required string Password { get; set; }
    }
}
