using NotificationSystem.Entities;

namespace NotificationSystem.Senders;

public sealed class ZaloSender(
    ILogger<ZaloSender> logger
    //, IOptions<ZaloSettings> options
    ) : INotificationSender
{
    private readonly ILogger<ZaloSender> _logger = logger;
    //private readonly ZaloSettings _zaloSettings = options.Value;

    public NotificationType Type => NotificationType.Zalo;

    public async Task Send(string message, User user)
    {
        //_logger.LogInformation("Attempting to send zalo message to {to}...", user.ZaloId);

        await Task.Delay(1000);

        _logger.LogInformation("Send zalo success");
    }
}
