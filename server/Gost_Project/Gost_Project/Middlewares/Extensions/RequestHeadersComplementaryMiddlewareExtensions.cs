namespace Anomaly.Middlewares.Extensions
{
    public static class RequestHeadersComplementaryMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestHeadersComplementaryMiddleware(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(nameof(app));

            return app.UseMiddleware<RequestHeadersComplementaryMiddleware>();
        }
    }
}
