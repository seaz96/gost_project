using Gost_Project.Data.Entities.Constraints;
using Gost_Project.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Models
{
    public class LoginModel
    {
        public required string Login {  get; set; }

        [MinLength(LoginModelConstraints.PASSWORD_MIN_LENGTH)]
        public required string Password { get; set; }
    }
}
