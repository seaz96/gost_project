using System.ComponentModel.DataAnnotations;
using GostStorage.Constraints;
using GostStorage.Helpers;

namespace GostStorage.Models.Accounts
{
    public class RegisterModel
    {
        [RegularExpression(RegexHelper.EmailValidationRegex)]
        public required string Login { get; set; }

        [MinLength(LoginModelConstraints.NAME_MIN_LENGTH)]
        public required string Name { get; set; }

        [MinLength(LoginModelConstraints.PASSWORD_MIN_LENGTH)]
        [MaxLength(LoginModelConstraints.PASSWORD_MAX_LENGTH)]
        public required string Password { get; set; }
        
        public string? OrgName { get; set; }

        public string? OrgBranch { get; set; }
        
        public string? OrgActivity { get; set; }
    }
}
