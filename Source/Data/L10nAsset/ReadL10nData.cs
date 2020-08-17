using System.Runtime.CompilerServices;

namespace VisualNovelData.Data
{
    public readonly struct ReadL10nData
    {
        public bool HasData { get; }

        private readonly L10nData data;

        public ILanguageList Languages
            => this.data.Languages;

        public IL10nTextDictionary L10nTexts
            => this.data.L10nTexts;

        public IContentDictionary Contents
            => this.data.Contents;

        private ReadL10nData(L10nData data)
        {
            this.data = data ?? _empty;
            this.HasData = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private L10nData GetData()
            => this.HasData ? this.data : _empty;

        public L10nTextRow GetText(string id)
            => GetData().GetText(id);

        public ContentRow GetContent(int id)
            => GetData().GetContent(id);

        private static readonly L10nData _empty = new L10nData();

        public static implicit operator ReadL10nData(L10nData data)
            => new ReadL10nData(data);
    }
}