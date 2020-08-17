using System.Runtime.CompilerServices;

namespace VisualNovelData.Data
{
    public readonly struct ReadCharacterData
    {
        public bool HasData { get; }

        private readonly CharacterData data;

        public ILanguageList Languages
            => this.data.Languages;

        public ICharacterDictionary Characters
            => this.data.Characters;

        public IContentDictionary Contents
            => this.data.Contents;

        private ReadCharacterData(CharacterData data)
        {
            this.data = data ?? _empty;
            this.HasData = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CharacterData GetData()
            => this.HasData ? this.data : _empty;

        public CharacterRow GetCharacter(string id)
            => GetData().GetCharacter(id);

        public ContentRow GetContent(int id)
            => GetData().GetContent(id);

        private static readonly CharacterData _empty = new CharacterData();

        public static implicit operator ReadCharacterData(CharacterData data)
            => new ReadCharacterData(data);
    }
}