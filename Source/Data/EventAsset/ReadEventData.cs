namespace VisualNovelData.Data
{
    public readonly struct ReadEventData
    {
        private readonly EventData data;

        public IEventDictionary Events
            => this.data.Events;

        private ReadEventData(EventData data)
        {
            this.data = data;
        }

        public EventRow GetEvent(string id)
            => this.data.GetEvent(id);

        public static implicit operator ReadEventData(EventData data)
            => new ReadEventData(data);
    }
}