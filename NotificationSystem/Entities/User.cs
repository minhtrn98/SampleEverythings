using NotificationSystem.Senders;

namespace NotificationSystem.Entities;

public sealed class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TelegramId { get; set; } = string.Empty;

    public NotificationType EnabledNotificationTypes { get; set; }
}
