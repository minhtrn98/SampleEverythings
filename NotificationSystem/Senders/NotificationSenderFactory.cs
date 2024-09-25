namespace NotificationSystem.Senders;

public sealed class NotificationSenderFactory(IServiceProvider serviceProvider) : INotificationSenderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public INotificationSender CreateSender(NotificationType type)
    {
        return _serviceProvider
            .GetServices<INotificationSender>()
            .Where(s => s.Type == type)
            .First();
    }
}