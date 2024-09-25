using NotificationSystem.Entities;

namespace NotificationSystem.Senders;

public sealed class SMSSender(
    ILogger<SMSSender> logger
    ) : INotificationSender
{
    private readonly ILogger<SMSSender> _logger = logger;

    public NotificationType Type => NotificationType.SMS;

    public async Task Send(string message, User user)
    {
        _logger.LogInformation("Attempting to send SMS message to {to}...", user.Phone);

        await Task.Delay(1000);

        _logger.LogInformation("Send SMS success");
    }
}
