using System.ComponentModel.DataAnnotations;
using Gost_Project.Data.Entities.Constraints;

namespace Gost_Project.Data.Models.Accounts
{
    public class LoginModel
    {
        public required string Login {  get; set; }

        [MinLength(LoginModelConstraints.PASSWORD_MIN_LENGTH)]
        public required string Password { get; set; }
    }
}
