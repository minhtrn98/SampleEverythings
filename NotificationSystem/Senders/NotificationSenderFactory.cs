namespace NotificationSystem.Senders;

public sealed class NotificationSenderFactory(IServiceProvider serviceProvider) : INotificationSenderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public INotificationSender CreateSender(NotificationType type)
    {
        IEnumerable<INotificationSender> services = _serviceProvider.GetServices<INotificationSender>();
        foreach (INotificationSender service in services)
        {
            if (service.Type == type)
            {
                return service;
            }
        }

        throw new ArgumentException("Invalid notification type");
    }
}