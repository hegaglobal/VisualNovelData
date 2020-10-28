using System.Collections.Generic;

namespace VisualNovelData.Parser
{
    using Data;

    public partial class VslRow
    {
        public override bool IsEmpty
            => string.IsNullOrEmpty(this.Key);

        public override string ToString()
        {
            this.stringBuilder.Clear();
            this.stringBuilder.Append(this.Key);

            return this.stringBuilder.ToString();
        }

        public L10nTextRow Parse(IL10nData data, L10nTextRow text, in Segment<string> languages, int row)
        {
            this.error.Clear();

            if (!this.IsEmpty)
            {
                var key = this.Key ?? string.Empty;

                if (!this.idRegex.IsMatch(key))
                {
                    this.error.AppendLine($"L10n key must only contain characters in {IdCharRange}. Current value: {key}");
                    return null;
                }

                if (data.L10nTexts.ContainsKey(key))
                    UnityEngine.Debug.LogWarning($"Vsl row {row}: L10n key has already existed");

                text = new L10nTextRow(row, this.Key ?? string.Empty);
                data.AddText(text);
                data.AddContent(new ContentRow(row, languages, this.Contents));
            }

            return text;
        }
    }
}