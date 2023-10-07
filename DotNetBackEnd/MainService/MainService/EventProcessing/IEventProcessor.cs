namespace DoAnTotNghiep.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
