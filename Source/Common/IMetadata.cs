namespace VisualNovelData
{
    public interface IMetadata
    {
        object Value { get; }
    }

    public interface ICastableMetadata : IMetadata
    {
        T As<T>();

        bool Is<T>();

        bool TryCast<T>(out T value);
    }
}