using GostStorage.Services;

namespace GostStorage.Middlewares;

public class SentryMiddleware(RequestDelegate next, ISentryService sentryService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exc)
        {
            await sentryService.NotifyAsync(exc, context);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Все сломалося(\nФиксики уже в пути и постараются починить...");
        }
    }
}