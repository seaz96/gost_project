using Anomaly.Middlewares;
using CorsairMessengerServer;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController(IPasswordHasher passwordHasher) : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        // SecurityHelper.GetAuthToken
        private void AddAuthorizationCookie(string token)
        {
            HttpContext.Response.Cookies.Append(RequestHeadersComplementaryMiddleware.AuthorizationTokenCookieName, token, new CookieOptions
            {
                MaxAge = AuthOptions.AuthTokenLifetime,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            });
        }
    }
}
