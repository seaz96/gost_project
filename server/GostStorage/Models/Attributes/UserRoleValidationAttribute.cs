using System.ComponentModel.DataAnnotations;
using GostStorage.Extensions;
using GostStorage.Navigations;

namespace GostStorage.Models.Attributes
{
    public class UserRoleValidationAttribute() : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string role)
            {
                return new ValidationResult(GetErrorMessage());
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                return new ValidationResult(GetErrorMessage());
            }

            if (!Enum.TryParse<UserRoles>(role.FirstCharToUpperNextToLower(), out var _))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private static string GetErrorMessage()
        {
            return "Invalid role";
        }
    }
}
