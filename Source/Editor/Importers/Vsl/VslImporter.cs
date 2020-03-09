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

    [ScriptedImporter(1, L10nAsset.Extension)]
    public sealed class VslImporter : CustomImporter<L10nAsset>
    {
        public const char Delimiter = ',';

        protected override L10nAsset Create(string assetPath, L10nAsset asset)
        {
            asset.ClearTexts();
            asset.ClearContents();

            var languages = LanguageListParser.Parse(assetPath, VslRow.Mapping.ContentsStartIndex);

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

        private CsvParser<VslRow> CreateParser(char columnSeparator, List<string> languages)
        {
            var parserOptions = new CsvParserOptions(true, columnSeparator);
            var mapper = new VslRow.Mapping(languages);

            return new CsvParser<VslRow>(parserOptions, mapper);
        }

        private L10nAsset Parse(CsvParser<VslRow> parser, FileStream stream, L10nAsset asset, List<string> languages)
        {
            var enumerator = parser.ReadFromStream(stream, Encoding.UTF8, true).GetEnumerator();
            var row = 0;
            var error = string.Empty;
            var logger = new StringBuilder();

            L10nTextRow text = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vslRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                text = vslRow.Parse(asset, text, languages, row);

                if (vslRow.IsError)
                {
                    error = vslRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsl row {row}: {error}");
                return null;
            }

            if (logger.Length > 0)
            {
                Debug.LogError(logger);
            }

            return asset;
        }
    }

    [CustomEditor(typeof(VslImporter))]
    public class VslImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();
        }
    }
}