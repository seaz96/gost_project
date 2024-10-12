using System.Text.RegularExpressions;

namespace GostStorage.API.Helpers
{
    public static partial class RegexHelper
    {
        public const string EmailValidationRegex = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,5}$";

        public static bool IsEmail(string input)
        {
            return EmailRegex().IsMatch(input);
        }

        [GeneratedRegex(EmailValidationRegex)]
        private static partial Regex EmailRegex();
    }
}
