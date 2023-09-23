using THUCTAPTOTNGHIEP.DTOM;

namespace MailService.Services.Contracts
{
    public interface IMailContent
    {   
        Task SendMailCreateOrderToAdmin(MailPublishedDto content);
        Task SendMailConfirmRegister(MailPublishedDto content);
        Task SendMailConfirmForgetPassword(MailPublishedDto content);
        Task SendMailConfirmChangeEmail(MailPublishedDto content);
        Task SendMailConfirmChangePassword(MailPublishedDto content);
        Task SendMailConfirmOrder(MailPublishedDto content);
        Task SendMailCancelOrder(MailPublishedDto content);
        Task SendMailSuccessOrder(MailPublishedDto content);
    }
}
