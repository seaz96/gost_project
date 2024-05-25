using GostStorage.Domain.Repositories;
using System.Security.Claims;

namespace GostStorage.API.Middlewares
{
    public class SessionValidityCheckMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IUserSessionsRepository userSessionsRepository)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var userIdAsString = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var sessionId = context.User.FindFirstValue(ClaimTypes.Sid);

                if (sessionId is not null && long.TryParse(userIdAsString, out var userId))
                {
                    if (!await IsSessionRegistered(userId, sessionId, userSessionsRepository))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        return;
                    }
                }
            }

            await _next(context);
        }

        private static async Task<bool> IsSessionRegistered(
            long userId, string sessionId, IUserSessionsRepository userSessionsRepository)
        {
            return await userSessionsRepository.IsSessionRegistered(userId, sessionId);
        }
    }
}
