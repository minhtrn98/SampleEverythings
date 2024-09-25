using Microsoft.Extensions.Options;
using NotificationSystem.Entities;
using NotificationSystem.Settings;

namespace NotificationSystem.Senders;

public sealed class TelegramSender(
    IHttpClientFactory httpClientFactory
    , IOptions<TelegramSettings> options
    , ILogger<TelegramSender> logger
    ) : INotificationSender
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("TelegramApi");
    private readonly TelegramSettings _telegramSettings = options.Value;
    private readonly ILogger<TelegramSender> _logger = logger;

    public NotificationType Type => NotificationType.Telegram;

    public async Task Send(string message, User user)
    {
        _logger.LogInformation("Attempting to send telegram message to {to}...", user.TelegramId);

        Dictionary<string, string> payload = new()
        {
            { "chat_id", user.TelegramId },
            { "text", message }
        };

        FormUrlEncodedContent content = new(payload);

        HttpResponseMessage response = await _httpClient.PostAsync("sendMessage", content);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Send mail success");
    }
}
