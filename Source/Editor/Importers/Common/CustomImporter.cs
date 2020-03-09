using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor;

namespace VisualNovelData.Importer.Editor
{
    public abstract class CustomImporter<T> : ScriptedImporter
        where T : ScriptableObject
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(ctx.assetPath);

            if (!asset)
                asset = ScriptableObject.CreateInstance<T>();

            asset = Create(ctx.assetPath, asset);

            if (!asset)
            {
                Debug.LogError($"Cannot import {ctx.assetPath}", this);
            }
            else
            {
                ctx.AddObjectToAsset("main obj", asset);
                ctx.SetMainObject(asset);
            }
        }

        protected abstract T Create(string assetPath, T asset);
    }
}