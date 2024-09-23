using NotificationSystem.Entities;
using NotificationSystem.Repositories;
using NotificationSystem.Senders;

namespace NotificationSystem.Services;

public sealed class NotificationService(
    INotificationSenderFactory senderFactory
    , IOutboxRepository outboxRepository
    , ILogger<NotificationService> logger
    ) : INotificationService
{
    private readonly INotificationSenderFactory _senderFactory = senderFactory;
    private readonly IOutboxRepository _outboxRepository = outboxRepository;
    private readonly ILogger<NotificationService> _logger = logger;

    public async Task SendNotification(string message, User user)
    {
        foreach (NotificationType type in Enum.GetValues(typeof(NotificationType)))
        {
            _logger.LogInformation("Checking if user {userId} has enabled notifications of type {type}", user.Id, type);
            if ((user.EnabledNotificationTypes & type) == type && type != NotificationType.None)
            {
                var sender = _senderFactory.CreateSender(type);
                await TrySendNotification(sender, message, user);
            }
        }
    }

    private async Task TrySendNotification(INotificationSender sender, string message, User user)
    {
        try
        {
            _logger.LogInformation("Sending notification of type {type} to user {userId}", sender.Type, user.Id);
            await sender.Send(message, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification of type {type} to user {userId}", sender.Type, user.Id);
            await _outboxRepository.SaveFailedNotification(sender.Type, message, user, ex.Message);
        }
    }
}
