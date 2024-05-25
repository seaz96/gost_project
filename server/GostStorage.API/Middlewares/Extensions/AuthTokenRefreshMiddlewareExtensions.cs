namespace GostStorage.API.Middlewares.Extensions
{
    public static class AuthTokenRefreshMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthTokenRefreshMiddleware(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(nameof(app));

            return app.UseMiddleware<AuthTokenRefreshMiddleware>();
        }
    }
}
