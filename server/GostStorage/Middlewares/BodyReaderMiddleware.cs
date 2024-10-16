using System.Text;
using GostStorage.Services;

namespace GostStorage.Middlewares;

public class BodyReaderMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post)
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            context.Items[SentryService.SerializedBodyItemsKey] = body.Replace("\n", string.Empty);
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
        }

        await next(context);
    }
}