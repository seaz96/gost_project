﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GostStorage.Services
{
    public static class AuthOptions
    {
        public const string AUTH_TOKEN_ISSUER = "Default";

        public const string AUTH_TOKEN_AUDIENCE = "Default";

        public static readonly TimeSpan AuthTokenLifetime = TimeSpan.FromDays(14);

        public static string SecurityKey { get; private set; } = "DefaultDevelopmentSecurityKey0123456789";

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
