using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GostStorage;

public static class AuthOptions
{
    public const string AuthTokenIssuer = "Default";
    public const string AuthTokenAudience = "Default";

    public static readonly TimeSpan AuthTokenLifetime = TimeSpan.FromDays(14);

    private static string? SecurityKey { get; set; }

    public static SymmetricSecurityKey SymmetricSecurityKey
    {
        get
        {
            ArgumentNullException.ThrowIfNull(SecurityKey);

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
        }
    }

    public static void Initialize(string securityKey)
    {
        ArgumentNullException.ThrowIfNull(securityKey);

        SecurityKey = securityKey;
    }
}