using NotificationSystem.Senders;

namespace NotificationSystem.Entities;

public sealed class FailedNotification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsReProcessSuccess { get; set; }
}
