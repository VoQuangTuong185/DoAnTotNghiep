using MailService.Services.Contracts;
using System.Net.Mail;
using THUCTAPTOTNGHIEP.DTOM;

namespace MailService.Services.Business
{   
    public class MailContent : IMailContent
    {
        private readonly IMailService _mailService;
        public MailContent(IMailService mailService) 
        {
            _mailService = mailService;
        }
        public async Task SendMailCreateOrderToAdmin(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("CreateOrder.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.UserName);
            emailTemplate = emailTemplate.Replace("{1}", content.Content);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
        public async Task SendMailCancelOrder(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("CancelOrder.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.UserName);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
        public async Task SendMailConfirmChangeEmail(MailPublishedDto content)
        {
            try
            {
                string emailTemplate = _mailService.GetEmail("ConfirmChangeEmail.html");
                var client = _mailService.GetEmailClient();
                string sysEmail = Config.Email;
                MailMessage mail = new MailMessage
                {
                    IsBodyHtml = true,
                    Subject = content.Subject,
                    From = new MailAddress(sysEmail, content.Title),
                };
                var toEmail = new MailAddress(content.Email);
                mail.To.Add(toEmail);
                emailTemplate = emailTemplate.Replace("{0}", content.Email);
                emailTemplate = emailTemplate.Replace("{1}", content.Content);
                mail.Body = emailTemplate;
                await client.SendMailAsync(mail);
            }
            catch(Exception ex)
            {

            }
        }
        public async Task SendMailConfirmChangePassword(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("ConfirmChangePassword.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.UserName);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
        public async Task SendMailConfirmForgetPassword(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("ConfirmForgetPassword.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.Email);
            emailTemplate = emailTemplate.Replace("{1}", content.Content);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
        public async Task SendMailConfirmOrder(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("ConfirmOrder.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.UserName);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
        public async Task SendMailConfirmRegister(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("ConfirmRegister.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.UserName);
            emailTemplate = emailTemplate.Replace("{1}", content.Content);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
        public async Task SendMailSuccessOrder(MailPublishedDto content)
        {
            string emailTemplate = _mailService.GetEmail("SuccessOrder.html");
            var client = _mailService.GetEmailClient();
            string sysEmail = Config.Email;
            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = content.Subject,
                From = new MailAddress(sysEmail, content.Title),
            };
            var toEmail = new MailAddress(content.Email);
            mail.To.Add(toEmail);
            emailTemplate = emailTemplate.Replace("{0}", content.UserName);
            mail.Body = emailTemplate;
            await client.SendMailAsync(mail);
        }
    }
}
