using Newtonsoft.Json;
using Telegram.Bot;

namespace GostStorage.Services;

public class SentryService(string token, long chatId) : ISentryService
{
    public const string SerializedBodyItemsKey = "SerializedBodyItems";

    private readonly TelegramBotClient _botClient = new(token);

    public async Task NotifyAsync(Exception exception, HttpContext httpContext)
    {
        var request = httpContext.Request;
        var body = request.Method == HttpMethods.Post ? JsonConvert.SerializeObject(httpContext.Items[SerializedBodyItemsKey]) : string.Empty;

        await _botClient.SendTextMessageAsync(chatId,
            "@qwuipss443 @seaz96\n\n" +
            $"{exception.Message}\n\n{exception.StackTrace}\n\n" +
            "\nHeaders:\n" + string.Join(string.Empty, request.Headers.Select(h => $"{h.Key}: {h.Value}\n")) +
            "\nQuery:\n" + string.Join(string.Empty, request.Query.Select(p => $"{p.Key}: {p.Value}\n")) +
            "\nContentType: " + request.ContentType +
            "\n\nBody:\n" + body);
    }
}