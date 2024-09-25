namespace NotificationSystem.Settings;

public sealed class MailSettings
{
    public required string Host { get; init; } = string.Empty;
    public required int Port { get; init; }
    public required string Username { get; init; } = string.Empty;
    public required string Password { get; init; } = string.Empty;
    public required string DisplayName { get; init; } = string.Empty;
}
