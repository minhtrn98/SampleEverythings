using NotificationSystem.Entities;

namespace NotificationSystem.Senders;

public interface INotificationSender
{
    NotificationType Type { get; }
    Task Send(string message, User user);
}
