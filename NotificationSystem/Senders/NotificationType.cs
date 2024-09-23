namespace NotificationSystem.Senders;

[Flags]
public enum NotificationType
{
    None = 0,
    Email = 1 << 0,   // 1
    SMS = 1 << 1,     // 2
    Telegram = 1 << 2 // 4
    // Add more types as needed
}
