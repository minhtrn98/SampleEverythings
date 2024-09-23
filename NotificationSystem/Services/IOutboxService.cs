namespace NotificationSystem.Services;

public interface IOutboxService
{
    Task RetryFailedNotifications();
}
