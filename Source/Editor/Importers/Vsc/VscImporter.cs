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

    [ScriptedImporter(1, CharacterAsset.Extension)]
    public sealed class VscImporter : CustomImporter<CharacterAsset>
    {
        protected override CharacterAsset Create(string assetPath, CharacterAsset asset)
        {
            asset.ClearCharacters();
            asset.ClearContents();

            var languages = LanguageListParser.Parse(assetPath, VscRow.Mapping.ContentsStartIndex);

            if (languages.Count > 0)
            {
                var csvParser = CreateParser(languages);
                var csvData = File.ReadAllText(assetPath);

                asset = Parse(csvParser, csvData, asset, languages);
                asset.AddLanguages(languages);
            }

            return asset;
        }

        private Parser<VscRow> CreateParser(List<string> languages)
        {
            var mapping = new VscRow.Mapping(languages);
            return CsvParser.Create<VscRow, VscRow.Mapping>(mapping);
        }

        private CharacterAsset Parse(Parser<VscRow> parser, string csvData, CharacterAsset asset, List<string> languages)
        {
            var enumerator = parser.Parse(csvData).GetEnumerator();
            var row = 0;
            var error = string.Empty;
            var logger = new StringBuilder();

            CharacterRow character = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vscRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                character = vscRow.Parse(asset, character, languages, row);

                if (vscRow.IsError)
                {
                    error = vscRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsc row {row}: {error}");
                return null;
            }

            if (logger.Length > 0)
            {
                Debug.LogError(logger);
            }

            return asset;
        }
    }

    [CustomEditor(typeof(VscImporter))]
    public class VscImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();
        }
    }
}