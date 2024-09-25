using NotificationSystem.Entities;
using NotificationSystem.Senders;

namespace NotificationSystem.Repositories;

public interface IOutboxRepository
{
    Task SaveFailedNotification(NotificationType type, string message, User user, string errorMessage);
    Task<IEnumerable<FailedNotification>> GetPendingNotifications();
    Task MarkAsProcessed(FailedNotification entry);
    Task IncreaseFailedCount(FailedNotification entry);
}
