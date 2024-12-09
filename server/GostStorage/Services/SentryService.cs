using System.Diagnostics;
using GostStorage.Extensions;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GostStorage.Services;

public class SentryService(string token, long chatId) : ISentryService
{
    public const string SerializedBodyItemsKey = "SerializedBodyItems";

    private readonly long _chatId = chatId;
    private readonly TelegramBotClient _botClient = new(token);

    public async Task NotifyAsync(Exception exception, HttpContext httpContext)
    {
        var request = httpContext.Request;
        var body = request.Method == HttpMethods.Post
            ? JsonConvert.SerializeObject(httpContext.Items[SerializedBodyItemsKey])
            : string.Empty;

        await _botClient.SendTextMessageAsync(_chatId,
            "@qwuipss443 @seaz96\n\n" +
            $"{exception.Message}\n\n"
            + $"Trace id: {Activity.Current?.TraceId}");

        await _botClient.SendDocumentAsync(_chatId,
            new InputFileStream(
                exception.StackTrace?.ToStream()
                ?? "Exception contains no stacktrace".ToStream(),
                "stacktrace.txt"));

        await _botClient.SendDocumentAsync(_chatId,
            new InputFileStream(body.ToStream(), "body.txt"));
    }
}