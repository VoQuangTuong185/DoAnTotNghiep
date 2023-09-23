using System.Net.Mail;

namespace MailService.Services.Contracts
{
    public interface IMailService
    {
        SmtpClient GetEmailClient();
        string GetEmail(string emailTemplateName);
    }
}
