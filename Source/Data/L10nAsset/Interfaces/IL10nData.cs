using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface IL10nData
    {
        ILanguageList Languages { get; }

        IL10nTextDictionary L10nTexts { get; }

        IContentDictionary Contents { get; }

        L10nTextRow GetText(string id);

        void AddText(L10nTextRow text);

        void ClearTexts();

        void AddLanguage(string language);

        void AddLanguages(in Segment<string> languages);

        void ClearLanguages();

        ContentRow GetContent(int id);

        void AddContent(ContentRow content);

        void RemoveContent(int id);

        void ClearContents();
    }
}