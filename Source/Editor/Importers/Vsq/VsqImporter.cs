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

    [ScriptedImporter(1, QuestAsset.Extension)]
    public class VsqImporter : CustomImporter<QuestAsset>
    {
        public const char Delimiter = ',';

        private readonly EventParser eventParser = new EventParser();

        protected override QuestAsset Create(string assetPath, QuestAsset asset)
        {
            asset.Clear();

            var csvParser = CreateParser(Delimiter);

            using (var stream = File.Open(assetPath, FileMode.Open, FileAccess.Read))
            {
                asset = Parse(csvParser, stream, asset);
            }

            return asset;
        }

        private CsvParser<VsqRow> CreateParser(char columnSeparator)
        {
            var parserOptions = new CsvParserOptions(true, columnSeparator);
            var mapper = new VsqRow.Mapping();

            return new CsvParser<VsqRow>(parserOptions, mapper);
        }

        private QuestAsset Parse(CsvParser<VsqRow> parser, FileStream stream, QuestAsset asset)
        {
            var enumerator = parser.ReadFromStream(stream, Encoding.UTF8, true).GetEnumerator();
            var row = 0;
            var error = string.Empty;

            QuestRow quest = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vsqRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                quest = vsqRow.Parse(asset, quest, this.eventParser, row);

                if (vsqRow.IsError)
                {
                    error = vsqRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsq row {row}: {error}");
                return null;
            }

            return asset;
        }
    }

    [CustomEditor(typeof(VsqImporter))]
    public class VsqImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();
        }
    }
}