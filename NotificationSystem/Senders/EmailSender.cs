using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using NotificationSystem.Entities;
using System.Net;

namespace NotificationSystem.Senders;

public sealed class EmailSender(ILogger<EmailSender> logger) : INotificationSender
{
    private readonly ILogger<EmailSender> _logger = logger;

    public NotificationType Type => NotificationType.Email;

    public async Task Send(string message, User user)
    {
        _logger.LogInformation("Attempting to send mail to {to}...", user.Email);

        MimeMessage mimeMessage = new();
        mimeMessage.From.Add(new MailboxAddress("Sample everything", "trancongminh503@gmail.com"));
        mimeMessage.To.Add(MailboxAddress.Parse(user.Email));
        mimeMessage.Subject = $"Test send mail";

        mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };

        using SmtpClient smtp = new();
        await smtp.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(new NetworkCredential("trancongminh503@gmail.com", "yeap jryg dkvt qfxl"));
        await smtp.SendAsync(mimeMessage);
        await smtp.DisconnectAsync(true);

        _logger.LogInformation("Send mail success");
    }
}
