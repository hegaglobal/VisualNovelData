using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.Text;
using UnityEditor;

namespace VisualNovelData.Importer.Editor
{
    using Data;
    using Parser;

    [ScriptedImporter(1, NovelAsset.Extension)]
    public class VsnImporter : CustomImporter<NovelAsset>
    {
        private readonly CommandParser commandParser = new CommandParser();
        private readonly IArrayParser<int> intArrayParser = new ArrayParser<int, IntParser>();

        protected override NovelAsset Create(string assetPath, NovelAsset asset)
        {
            asset.ClearLanguages();
            asset.ClearConversations();

            var languages = LanguageListParser.Parse(assetPath, VsnRow.Mapping.ContentsStartIndex);

            if (languages.Count > 0)
            {
                var csvParser = CreateParser(languages);
                var csvData = File.ReadAllText(assetPath);

                asset = Parse(csvParser, csvData, asset, languages);
                asset.AddLanguages(languages);
            }

            return asset;
        }

        private Parser<VsnRow> CreateParser(List<string> languages)
        {
            var mapping = new VsnRow.Mapping(languages);
            return CsvParser.Create<VsnRow, VsnRow.Mapping>(mapping);
        }

        private NovelAsset Parse(Parser<VsnRow> parser, string csvData, NovelAsset asset, List<string> languages)
        {
            var enumerator = parser.Parse(csvData).GetEnumerator();
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
                                                        goToList, this.commandParser, this.intArrayParser, logger);

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