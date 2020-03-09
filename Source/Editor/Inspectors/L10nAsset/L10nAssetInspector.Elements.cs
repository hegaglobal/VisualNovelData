namespace VisualNovelData.Data.Editor
{
    public partial class L10nAssetInspector
    {
        private class L10nAssetElement : Folder
        {
            public L10nAssetElement() : base("l10n-asset")
            {
            }
        }

        private class L10nTextElement : Folder
        {
            public L10nTextElement() : base("l10n-text")
            {
                this.value = false;
            }
        }
    }
}