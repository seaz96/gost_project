using GostStorage.Services.Constraints;
using System.ComponentModel.DataAnnotations;

namespace GostStorage.Services.Models.Accounts
{
    public class LoginModel
    {
        public required string Login { get => _login.ToLower(); set => _login = value; }

        [MinLength(
            LoginModelConstraints.PASSWORD_MIN_LENGTH,
            ErrorMessage = "Пароль слишком короткий")]
        public required string Password { get; set; }

        private string _login = null!;
    }
}
