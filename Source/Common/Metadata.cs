namespace VisualNovelData
{
    public readonly struct Metadata<T> : IMetadata
    {
        private readonly T value;

        public object Value => this.value;

        public Metadata(T value)
        {
            this.value = value;
        }

        public static implicit operator Metadata<T>(T value)
            => new Metadata<T>(value);

        public static implicit operator T(in Metadata<T> value)
            => value.value;

        public static implicit operator Metadata(in Metadata<T> value)
            => new Metadata(value.value);
    }

    public readonly struct Metadata : IMetadata
    {
        public object Value { get; }

        public Metadata(object value)
        {
            this.Value = value;
        }

        public static Metadata None { get; } = new Metadata(string.Empty);
    }

    public static class MetadataExtensions
    {
        public static Metadata AsMetadata<T>(this T self)
        {
            if (self == null)
                return Metadata.None;

            return new Metadata<T>(self);
        }
    }
}