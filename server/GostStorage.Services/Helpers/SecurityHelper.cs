using GostStorage.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                    expires: GetNewTokenExpireDateTime(),
                    signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public static string GetAuthToken(JwtSecurityToken token)
        {
            var jwt = new JwtSecurityToken(
                    claims: token.Claims,
                    issuer: AuthOptions.AUTH_TOKEN_ISSUER,
                    audience: AuthOptions.AUTH_TOKEN_ISSUER,
                    expires: GetNewTokenExpireDateTime(),
                    signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var newToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return newToken;
        }

        public static DateTime GetNewTokenExpireDateTime()
        {
            return DateTime.UtcNow + AuthOptions.AuthTokenLifetime;
        }
    }
}
