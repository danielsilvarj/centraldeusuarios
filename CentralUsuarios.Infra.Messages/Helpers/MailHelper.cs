using CentralUsuarios.Infra.Messages.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CentralUsuarios.Infra.Messages.Helpers;

public class MailHelper
{
    private readonly MailSettings _mailSettings;

    public MailHelper(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public void Send(string mailTo, string subject, string body)
    {
        var mailMessage = new MailMessage(_mailSettings.Email, mailTo);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;

        var smtpClient = new SmtpClient(_mailSettings.Smtp, _mailSettings.Port);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
        smtpClient.Send(mailMessage);
    }
}
