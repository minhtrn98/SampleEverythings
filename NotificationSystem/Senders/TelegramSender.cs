using NotificationSystem.Entities;

namespace NotificationSystem.Senders;

public sealed class TelegramSender : INotificationSender
{
    public NotificationType Type => NotificationType.Telegram;

    public async Task Send(string message, User user)
    {
        // Code to send Telegram message
    }
}
