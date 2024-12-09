namespace GostStorage.Middlewares.Extensions;

public static class SentryMiddlewareExtensions
{
    public static IApplicationBuilder UseSentry(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(nameof(app));

        return app.UseMiddleware<SentryMiddleware>();
    }
}