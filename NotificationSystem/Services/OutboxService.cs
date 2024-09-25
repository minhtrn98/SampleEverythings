using NotificationSystem.Entities;
using NotificationSystem.Repositories;
using NotificationSystem.Senders;

namespace NotificationSystem.Services;

public sealed class OutboxService(
    INotificationSenderFactory senderFactory
    , IOutboxRepository outboxRepository
    , IUserRepository userRepository
    , ILogger<OutboxService> logger
    ) : IOutboxService
{
    private readonly INotificationSenderFactory _senderFactory = senderFactory;
    private readonly IOutboxRepository _outboxRepository = outboxRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<OutboxService> _logger = logger;

    public async Task RetryFailedNotifications()
    {
        _logger.LogInformation("Retrying failed notifications");
        IEnumerable<FailedNotification> failedNotifications = await _outboxRepository.GetPendingNotifications();
        User? user = null;
        foreach (FailedNotification entry in failedNotifications)
        {
            INotificationSender sender = _senderFactory.CreateSender(entry.NotificationType);
            try
            {
                user ??= await GetUserById(entry.UserId);
                await sender.Send(entry.Message, user);
                await _outboxRepository.MarkAsProcessed(entry);
                _logger.LogInformation("Successfully sent notification of type {type} to user {userId}", sender.Type, user?.Id);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to send notification of type {type} to user {userId}", sender.Type, user?.Id);
                await _outboxRepository.IncreaseFailedCount(entry);
            }
        }
    }

    private async Task<User> GetUserById(int userId)
    {
        return await _userRepository.GetUserById(userId) ?? throw new Exception();
    }
}
