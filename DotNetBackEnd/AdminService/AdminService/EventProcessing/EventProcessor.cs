using DoAnTotNghiep.DTOM;
using System.Text.Json;
using WebAppAPI.Services.Contracts;
using WebAppAPI.Services.Business;

namespace DoAnTotNghiep.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ILog _ILog;
        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _ILog = Log.GetInstance;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.MailPublished:
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
                default:
                    _ILog.LogException("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
        enum EventType
        {
            MailPublished,
            Undetermined
        }
    }
}
