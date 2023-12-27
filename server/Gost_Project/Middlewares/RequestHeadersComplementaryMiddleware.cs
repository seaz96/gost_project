using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Anomaly.Middlewares
{
    public class RequestHeadersComplementaryMiddleware(RequestDelegate next)
    {
        public const string AuthorizationTokenCookieName = ".AspNetCore.Application.Marker";

        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var authToken = context.Request.Cookies[AuthorizationTokenCookieName];

            if (!authToken.IsNullOrEmpty())
            {
                context.Request.Headers.Authorization = $"{JwtBearerDefaults.AuthenticationScheme} {authToken}";
            }

            await _next(context);
        }
    }
}
