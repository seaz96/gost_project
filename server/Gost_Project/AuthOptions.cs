using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Gost_Project
{
    public static class AuthOptions
    {
        public const string AUTH_TOKEN_ISSUER = "Default";

        public const string AUTH_TOKEN_AUDIENCE = "Default";

        public static readonly TimeSpan AuthTokenLifetime = TimeSpan.FromDays(14);

        public static string? SecurityKey { get; private set; }

        public static SymmetricSecurityKey SymmetricSecurityKey
        {
            get
            {
                ArgumentNullException.ThrowIfNull(SecurityKey);

                return new(Encoding.UTF8.GetBytes(SecurityKey));
            }
        }

        public static void Initialize(string securityKey)
        {
            ArgumentNullException.ThrowIfNull(securityKey);

            SecurityKey = securityKey;
        }
    }
}
