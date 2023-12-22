namespace Anomaly.Middlewares
{
    public class SecurityHeadersComplementaryMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Xss-Protection", "1");
            context.Response.Headers.Append("X-Frame-Options", "DENY");

            await _next(context);
        }
    }
}
