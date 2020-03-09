using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using TinyCsvParser;
using System.Text;
using UnityEditor;

namespace VisualNovelData.Importer.Editor
{
    using Data;
    using Parser;

    [ScriptedImporter(1, NovelAsset.Extension)]
    public class VsnImporter : CustomImporter<NovelAsset>
    {
        public const char Delimiter = ',';

        private readonly EventParser eventParser = new EventParser();

        protected override NovelAsset Create(string assetPath, NovelAsset asset)
        {
            asset.ClearLanguages();
            asset.ClearConversations();

            var languages = LanguageListParser.Parse(assetPath, VsnRow.Mapping.ContentsStartIndex);

            if (languages.Count > 0)
            {
                var csvParser = CreateParser(Delimiter, languages);

                using (var stream = File.Open(assetPath, FileMode.Open, FileAccess.Read))
                {
                    asset = Parse(csvParser, stream, asset, languages);
                    asset.AddLanguages(languages);
                }
            }

            return asset;
        }

        private CsvParser<VsnRow> CreateParser(char columnSeparator, List<string> languages)
        {
            var parserOptions = new CsvParserOptions(true, columnSeparator);
            var mapper = new VsnRow.Mapping(languages);

            return new CsvParser<VsnRow>(parserOptions, mapper);
        }

        private NovelAsset Parse(CsvParser<VsnRow> parser, FileStream stream, NovelAsset asset, List<string> languages)
        {
            var enumerator = parser.ReadFromStream(stream, Encoding.UTF8, true).GetEnumerator();
            var row = 0;
            var error = string.Empty;
            var logger = new StringBuilder();
            var goToList = new List<string>();

            ConversationRow conversation = null;
            DialogueRow dialogue = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vsnRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                (conversation, dialogue) = vsnRow.Parse(asset, conversation, dialogue, languages, row,
                                                        goToList, this.eventParser, logger);

                if (vsnRow.IsError)
                {
                    error = vsnRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsn row {row}: {error}");
                return null;
            }

            if (logger.Length > 0)
            {
                Debug.LogError(logger);
            }

            return asset;
        }
    }

    [CustomEditor(typeof(VsnImporter))]
    public class VsnImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();
        }
    }
}