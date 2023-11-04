using AutoMapper;
using Microsoft.CodeAnalysis;
using THUCTAPTOTNGHIEP.DTOM;
using System.Text.Json;
using MailService.Services.Contracts;
using MailService.Services.Business;

namespace THUCTAPTOTNGHIEP.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMailContent _mailContent;
        private ILog _ILog;
        public EventProcessor(IServiceScopeFactory scopeFactory, IMailContent mailContent)
        {
            _scopeFactory = scopeFactory;
            _mailContent = mailContent;
            _ILog = Log.GetInstance;
        }
        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.MailPublished:
                    await sendMail(message);
                    break;
                case EventType.CategoryPublished:
                    _ILog.LogException("--> Don't have any action!!");
                    break;
                default:
                    break;
            }
        }
        private EventType DetermineEvent(string notifcationMessage)
        {
            _ILog.LogException("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch (eventType.Event)
            {
                case "Mail_Published":
                    _ILog.LogException("--> Mail Published Event Detected");
                    return EventType.MailPublished;
                case "Category_Published":
                    _ILog.LogException("--> Category Published Event Detected");
                    return EventType.CategoryPublished;
                default:
                    _ILog.LogException("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
        private async Task sendMail(string mailPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var categoryPublishedDto = JsonSerializer.Deserialize<MailPublishedDto>(mailPublishedMessage);
                try
                {
                    switch (categoryPublishedDto.Business)
                    {
                        case "CreateOrder":
                            await _mailContent.SendMailCreateOrderToAdmin(categoryPublishedDto);
                            break;
                        case "CancelOrder":
                            await _mailContent.SendMailCancelOrder(categoryPublishedDto);
                            break;
                        case "ConfirmChangeEmail":
                            await _mailContent.SendMailConfirmChangeEmail(categoryPublishedDto);
                            break;
                        case "ConfirmChangePassword":
                            await _mailContent.SendMailConfirmChangePassword(categoryPublishedDto);
                            break;
                        case "ConfirmForgetPassword":
                            await _mailContent.SendMailConfirmForgetPassword(categoryPublishedDto);
                            break;
                        case "ConfirmOrder":
                            await _mailContent.SendMailConfirmOrder(categoryPublishedDto);
                            break;
                        case "ConfirmRegister":
                            await _mailContent.SendMailConfirmRegister(categoryPublishedDto);
                            break;
                        case "SuccessOrder":
                            await _mailContent.SendMailSuccessOrder(categoryPublishedDto);
                            break;
                        default:
                            break;
                    }                   
                }
                catch (Exception ex)
                {
                    _ILog.LogException($"--> Could not send mail {ex.Message}");
                }
            }
        }
        enum EventType
        {
            CategoryPublished,
            MailPublished,
            Undetermined
        }
    }
}
