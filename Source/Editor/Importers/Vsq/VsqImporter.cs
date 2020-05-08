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
        private readonly EventParser eventParser = new EventParser();

        protected override QuestAsset Create(string assetPath, QuestAsset asset)
        {
            asset.Clear();

            var csvParser = CreateParser();
            var csvData = File.ReadAllText(assetPath);

            asset = Parse(csvParser, csvData, asset);
            return asset;
        }

        private Parser<VsqRow> CreateParser()
        {
            var mapping = new VsqRow.Mapping();
            return CsvParser.Create<VsqRow, VsqRow.Mapping>(mapping);
        }

        private QuestAsset Parse(Parser<VsqRow> parser, string csvData, QuestAsset asset)
        {
            var enumerator = parser.Parse(csvData).GetEnumerator();
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