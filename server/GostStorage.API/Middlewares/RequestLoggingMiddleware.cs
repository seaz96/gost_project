using System.Text;
using Serilog;

namespace GostStorage.API.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private readonly string[] _actions = ["login", "register", "restore-password", "change-password"];

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_actions.Any(action => context.Request.Path.Value != null && context.Request.Path.Value.Contains(action)))
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            if (body.Length > 0)
            {
                Log.Logger.Information("HTTP request body: " + body);
                var byteArray = Encoding.UTF8.GetBytes(body);
                var stream = new MemoryStream(byteArray);
                context.Request.Body = stream;
            }
        }

        await _next.Invoke(context);
    }
}