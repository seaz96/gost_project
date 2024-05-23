using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GostStorage.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace GostStorage.Services.Helpers
{
    public class SecurityHelper
    {
        public static string GetAuthToken(UserEntity user, string sessionId)
        {
            var userId = user.Id.ToString();
            var role = user.Role.ToString();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.Sid, sessionId),
            };

            var jwt = new JwtSecurityToken(
                    claims: claims,
                    issuer: AuthOptions.AUTH_TOKEN_ISSUER,
                    audience: AuthOptions.AUTH_TOKEN_ISSUER,
                    expires: DateTime.UtcNow + AuthOptions.AuthTokenLifetime,
                    signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
    }
}
