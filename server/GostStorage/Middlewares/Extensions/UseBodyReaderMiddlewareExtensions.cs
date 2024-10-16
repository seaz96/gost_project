namespace GostStorage.Middlewares.Extensions;

public static class UseBodyReaderMiddlewareExtensions
{
    public static IApplicationBuilder UseBodyReader(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(nameof(app));

        return app.UseMiddleware<BodyReaderMiddleware>();
    }
}