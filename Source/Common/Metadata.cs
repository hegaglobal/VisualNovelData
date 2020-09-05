using System.Collections.Generic;

namespace VisualNovelData
{
    public readonly struct Metadata<T> : ICastableMetadata
    {
        private readonly T value;

        public object Value => this.value;

        public Metadata(T value)
        {
            this.value = value;
        }

        public override bool Equals(object obj)
            => obj is Metadata<T> other &&
               EqualityComparer<T>.Default.Equals(this.value, other.value);

        public override int GetHashCode()
            => -1584136870 + EqualityComparer<T>.Default.GetHashCode(this.value);

        public U As<U>()
        {
            if (this.Value is U valueU)
                return valueU;

            return default;
        }

        public bool Is<U>()
            => this.Value is U;

        public bool TryCast<U>(out U value)
        {
            if (this.Value is U valueU)
            {
                value = valueU;
                return true;
            }

            value = default;
            return false;
        }

        public static implicit operator Metadata<T>(T value)
            => new Metadata<T>(value);

        public static implicit operator T(in Metadata<T> value)
            => value.value;

        public static implicit operator Metadata(in Metadata<T> value)
            => new Metadata(value.value);
    }

    public readonly struct Metadata : ICastableMetadata
    {
        public object Value { get; }

        public Metadata(object value)
        {
            this.Value = value;
        }

        public T As<T>()
        {
            if (this.Value is T valueT)
                return valueT;

            return default;
        }

        public bool Is<T>()
            => this.Value is T;

        public bool TryCast<T>(out T value)
        {
            if (this.Value is T valueT)
            {
                value = valueT;
                return true;
            }

            value = default;
            return false;
        }

        public override bool Equals(object obj)
            => obj is Metadata other &&
               EqualityComparer<object>.Default.Equals(this.Value, other.Value);

        public override int GetHashCode()
            => -1937169414 + EqualityComparer<object>.Default.GetHashCode(this.Value);

        private static readonly object _none = new object();

        public static Metadata None { get; } = new Metadata(_none);
    }

    public static class MetadataExtensions
    {
        public static Metadata AsMetadata<T>(this T self)
        {
            if (self == null)
                return Metadata.None;

            return new Metadata<T>(self);
        }

        public static T As<T>(this IMetadata self)
        {
            if (self?.Value is T valueT)
                return valueT;

            return default;
        }

        public static bool Is<T>(this IMetadata self)
            => self?.Value is T;

        public static bool TryCast<T>(this IMetadata self, out T value)
        {
            if (self?.Value is T valueT)
            {
                value = valueT;
                return true;
            }

            value = default;
            return false;
        }
    }
}