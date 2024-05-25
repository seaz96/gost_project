using GostStorage.Services.Constraints;
using GostStorage.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace GostStorage.Services.Models.Accounts
{
    public class RegisterModel
    {
        [RegularExpression(
            RegexHelper.EmailValidationRegex,
            ErrorMessage = "Некорректный адрес электронной почты")]
        public required string Login { get => _login.ToLower(); set => _login = value; }

        [MinLength(LoginModelConstraints.NAME_MIN_LENGTH)]
        public required string Name { get; set; }

        [MinLength(
            LoginModelConstraints.PASSWORD_MIN_LENGTH,
            ErrorMessage = "Пароль слишком короткий")]
        [MaxLength(
            LoginModelConstraints.PASSWORD_MAX_LENGTH,
            ErrorMessage = "Пароль слишком длинный")]
        public required string Password { get; set; }

        public string? OrgName { get; set; }

        public string? OrgBranch { get; set; }

        public string? OrgActivity { get; set; }

        private string _login = null!;
    }
}
