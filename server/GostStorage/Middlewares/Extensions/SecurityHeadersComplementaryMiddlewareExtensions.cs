namespace GostStorage.Middlewares.Extensions;

public static class SecurityHeadersComplementaryMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeadersComplementary(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(nameof(app));

        return app.UseMiddleware<SecurityHeadersComplementaryMiddleware>();
    }
}