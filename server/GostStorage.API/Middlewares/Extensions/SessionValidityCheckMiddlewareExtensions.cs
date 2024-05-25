namespace GostStorage.API.Middlewares.Extensions
{
    public static class SessionValidityCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionValidityCheckMiddlewareMiddleware(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(nameof(app));

            return app.UseMiddleware<SessionValidityCheckMiddleware>();
        }
    }
}
