using System.Runtime.CompilerServices;

namespace VisualNovelData.Data
{
    public readonly struct ReadEventData
    {
        public bool HasData { get; }

        private readonly EventData data;

        public IEventDictionary Events
            => this.data.Events;

        private ReadEventData(EventData data)
        {
            this.data = data ?? _empty;
            this.HasData = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private EventData GetData()
            => this.HasData ? this.data : _empty;

        public EventRow GetEvent(string id)
            => GetData().GetEvent(id);

        private static readonly EventData _empty = new EventData();

        public static implicit operator ReadEventData(EventData data)
            => new ReadEventData(data);
    }
}