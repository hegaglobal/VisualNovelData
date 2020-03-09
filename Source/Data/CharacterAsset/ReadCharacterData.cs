namespace VisualNovelData.Data
{
    public readonly struct ReadCharacterData
    {
        private readonly CharacterData data;

        public ILanguageList Languages
            => this.data.Languages;

        public ICharacterDictionary Characters
            => this.data.Characters;

        public IContentDictionary Contents
            => this.data.Contents;

        private ReadCharacterData(CharacterData data)
        {
            this.data = data;
        }

        public CharacterRow GetCharacter(string id)
            => this.data.GetCharacter(id);

        public ContentRow GetContent(int id)
            => this.data.GetContent(id);

        public static implicit operator ReadCharacterData(CharacterData data)
            => new ReadCharacterData(data);
    }
}