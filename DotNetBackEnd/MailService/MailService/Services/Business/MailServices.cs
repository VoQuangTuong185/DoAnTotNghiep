using MailService.Services.Contracts;
using System.Net.Mail;

namespace MailService.Services.Business
{
    public class MailServices : IMailService
    {
        public MailServices()
        {
        }
        SmtpClient IMailService.GetEmailClient()
        {
            var sysMail = Config.Email;
            var password = Config.Password;
            return new SmtpClient
            {
                Host = "smtp.office365.com",
                UseDefaultCredentials = false,
                EnableSsl = true,
                TargetName = "STARTTLS/smtp.office365.com",
                Credentials = new System.Net.NetworkCredential(sysMail, password),
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };
        }
        public string GetEmail(string emailTemplateName)
        {
            string emailTemplate = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine("EmailTemplates", emailTemplateName)))
            {
                emailTemplate = reader.ReadToEnd();
            }
            return emailTemplate;
        }
    }
}
