namespace GostStorage.API.Middlewares.Extensions
{
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(nameof(app));

            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
