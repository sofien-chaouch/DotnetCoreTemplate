namespace PlatformService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}