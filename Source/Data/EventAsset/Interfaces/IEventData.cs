namespace VisualNovelData.Data
{
    public interface IEventData
    {
        IEventDictionary Events { get; }

        EventRow GetEvent(string id);

        void AddEvent(EventRow @event);

        void Clear();
    }
}