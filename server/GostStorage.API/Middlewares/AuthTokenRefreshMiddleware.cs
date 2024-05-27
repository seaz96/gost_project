using GostStorage.Services;
using GostStorage.Services.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Serilog;

namespace GostStorage.API.Middlewares
{
    public class AuthTokenRefreshMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            string newToken = null!;

            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();

                    var rawToken = context.Request.Headers.Authorization
                        .ToString()[JwtBearerDefaults.AuthenticationScheme.Length..].Trim();

                    var securityToken = (JwtSecurityToken)handler.ReadToken(rawToken);

                    if (IsRefreshNeeded(securityToken.ValidTo))
                    {
                        newToken = SecurityHelper.GetAuthToken(securityToken);
                    }
                }
                catch (Exception exception)
                {
                    Log.Logger.Error("Error while refreshing token: {exception}", exception.Message);
                }
                // (NOTE) на случай если не прочитается securityToken,
                // секция не критическая, поэтому падать с каждого запроса в случае каких-либо
                // изменений в авторизации не очень хотелось бы, поэтому ничего не рефрешим,
                // пусть токен тухнет, юзер залогинится еще раз
            }

            if (newToken is not null)
            {
                context.Response.Headers.Authorization = newToken;
            }

            await _next.Invoke(context);
        }

        private static bool IsRefreshNeeded(DateTime dateTime)
        {
            return dateTime - DateTime.Now < TimeSpan.FromDays(AuthOptions.AuthTokenLifetime.Days / 2);
        }
    }
}
