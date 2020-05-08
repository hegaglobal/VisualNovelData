using System.Collections.Generic;

namespace VisualNovelData.Parser
{
    using Data;

    public partial class VscRow
    {
        public override bool IsEmpty
            => string.IsNullOrEmpty(this.Character);

        public override string ToString()
        {
            this.stringBuilder.Clear();
            this.stringBuilder.Append($"{this.Character} - {this.Avatar} - {this.Model}");

            return this.stringBuilder.ToString();
        }

        public CharacterRow Parse(ICharacterData data, CharacterRow character, in Segment<string> languages, int row)
        {
            this.error.Clear();

            if (!this.IsEmpty)
            {
                var id = this.Character ?? string.Empty;

                if (data.Characters.ContainsKey(id))
                    UnityEngine.Debug.LogWarning($"Vsc row {row}: Character id has already existed");

                character = new CharacterRow(
                    row,
                    this.Character ?? string.Empty,
                    this.Avatar ?? string.Empty,
                    this.Model ?? string.Empty,
                    this.Background ?? string.Empty
                );

                data.AddCharacter(character);
                data.AddContent(new ContentRow(row, languages, this.Contents));
            }

            return character;
        }
    }
}