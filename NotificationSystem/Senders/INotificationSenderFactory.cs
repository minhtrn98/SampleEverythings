namespace NotificationSystem.Senders;

public interface INotificationSenderFactory
{
    INotificationSender CreateSender(NotificationType type);
}
