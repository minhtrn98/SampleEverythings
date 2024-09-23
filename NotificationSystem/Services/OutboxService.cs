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
            }
            catch (Exception)
            {
                // Log and continue retrying later
            }
        }
    }

    private async Task<User> GetUserById(int userId)
    {
        return await _userRepository.GetUserById(userId) ?? throw new Exception();
    }
}
