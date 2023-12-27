namespace Anomaly.Middlewares.Extensions
{
    public static class SecurityHeadersComplementaryMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeadersComplementaryMiddleware(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(nameof(app));

            return app.UseMiddleware<SecurityHeadersComplementaryMiddleware>();
        }
    }
}
