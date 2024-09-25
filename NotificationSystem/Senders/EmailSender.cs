using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NotificationSystem.Entities;
using NotificationSystem.Settings;
using System.Net;

namespace NotificationSystem.Senders;

public sealed class EmailSender(
    ILogger<EmailSender> logger
    , IOptions<MailSettings> options
    ) : INotificationSender
{
    private readonly ILogger<EmailSender> _logger = logger;
    private readonly MailSettings _mailSettings = options.Value;

    public NotificationType Type => NotificationType.Email;

    public async Task Send(string message, User user)
    {
        _logger.LogInformation("Attempting to send mail to {to}...", user.Email);

        MimeMessage mimeMessage = new();
        mimeMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Username));
        mimeMessage.To.Add(MailboxAddress.Parse(user.Email));
        mimeMessage.Subject = $"Test send mail";

        mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };

        using SmtpClient smtp = new();
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(new NetworkCredential(_mailSettings.Username, _mailSettings.Password));
        await smtp.SendAsync(mimeMessage);
        await smtp.DisconnectAsync(true);

        _logger.LogInformation("Send mail success");
    }
}
