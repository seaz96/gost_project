namespace GostStorage.Middlewares.Extensions;

public static class SecurityHeadersComplementaryMiddlewareExtensions
{
    public static void UseSecurityHeadersComplementaryMiddleware(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(nameof(app));

        app.UseMiddleware<SecurityHeadersComplementaryMiddleware>();
    }
}