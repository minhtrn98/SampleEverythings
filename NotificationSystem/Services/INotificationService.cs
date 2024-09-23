using NotificationSystem.Entities;

namespace NotificationSystem.Services
{
    public interface INotificationService
    {
        Task SendNotification(string message, User user);
    }
}