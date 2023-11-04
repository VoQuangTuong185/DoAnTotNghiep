namespace THUCTAPTOTNGHIEP.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
}
