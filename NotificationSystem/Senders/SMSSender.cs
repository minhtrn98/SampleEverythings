using NotificationSystem.Entities;

namespace NotificationSystem.Senders;

public sealed class SMSSender : INotificationSender
{
    public NotificationType Type => NotificationType.SMS;

    public async Task Send(string message, User user)
    {
        // Code to send SMS
    }
}
