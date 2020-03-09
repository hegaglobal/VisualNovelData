namespace VisualNovelData.Data
{
    public readonly struct ReadL10nData
    {
        private readonly L10nData data;

        public ILanguageList Languages
            => this.data.Languages;

        public IL10nTextDictionary L10nTexts
            => this.data.L10nTexts;

        public IContentDictionary Contents
            => this.data.Contents;

        private ReadL10nData(L10nData data)
        {
            this.data = data;
        }

        public L10nTextRow GetText(string id)
            => this.data.GetText(id);

        public ContentRow GetContent(int id)
            => this.data.GetContent(id);

        public static implicit operator ReadL10nData(L10nData data)
            => new ReadL10nData(data);
    }
}