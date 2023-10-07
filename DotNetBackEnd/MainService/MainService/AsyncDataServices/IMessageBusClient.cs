using DoAnTotNghiep.DTOM;

namespace CategoryService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishMail(MailPublishedDto mailPublishDto);
    }
}
