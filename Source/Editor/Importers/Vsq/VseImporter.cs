using System.IO;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor;

namespace VisualNovelData.Importer.Editor
{
    using Data;
    using Parser;

    [ScriptedImporter(1, EventAsset.Extension)]
    public class VseImporter : CustomImporter<EventAsset>
    {
        private readonly CommandParser commandParser = new CommandParser();

        protected override EventAsset Create(string assetPath, EventAsset asset)
        {
            asset.Clear();

            var csvParser = CreateParser();
            var csvData = File.ReadAllText(assetPath);

            asset = Parse(csvParser, csvData, asset);
            return asset;
        }

        private Parser<VseRow> CreateParser()
        {
            var mapping = new VseRow.Mapping();
            return CsvParser.Create<VseRow, VseRow.Mapping>(mapping);
        }

        private EventAsset Parse(Parser<VseRow> parser, string csvData, EventAsset asset)
        {
            var enumerator = parser.Parse(csvData).GetEnumerator();
            var row = 0;
            var error = string.Empty;

            EventRow @event = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vseRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                @event = vseRow.Parse(asset, @event, this.commandParser, row);

                if (vseRow.IsError)
                {
                    error = vseRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vse row {row}: {error}");
                return null;
            }

            return asset;
        }
    }

    [CustomEditor(typeof(VseImporter))]
    public class VseImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();
        }
    }
}