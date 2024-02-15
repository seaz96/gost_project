using Gost_Project.Data.Entities.Constraints;
using Gost_Project.Data.Models.Attributes;
using Gost_Project.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Gost_Project.Data.Models
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
