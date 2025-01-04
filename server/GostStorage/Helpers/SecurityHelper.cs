using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GostStorage.Entities;
using Microsoft.IdentityModel.Tokens;

namespace GostStorage.Helpers;

public class SecurityHelper
{
    public static string GetAuthToken(User user)
    {
        var userId = user.Id.ToString();
        var role = user.Role.ToString();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role)
        };

        var jwt = new JwtSecurityToken(
            claims: claims,
            issuer: AuthOptions.AuthTokenIssuer,
            audience: AuthOptions.AuthTokenIssuer,
            expires: DateTime.UtcNow + AuthOptions.AuthTokenLifetime,
            signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }
}