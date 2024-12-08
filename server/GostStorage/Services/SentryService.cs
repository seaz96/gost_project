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
            $"{exception.Message}\n\n" 
            + httpContext.TraceIdentifier);
    }
}