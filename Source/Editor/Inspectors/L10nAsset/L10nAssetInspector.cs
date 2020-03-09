using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace VisualNovelData.Data.Editor
{
    [CustomEditor(typeof(L10nAsset), true)]
    public partial class L10nAssetInspector : UnityEditor.Editor
    {
        protected VisualElement root;
        protected L10nAsset asset;

        private void OnEnable()
        {
            this.asset = this.target as L10nAsset;
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.root = new VisualElement();
            CreateDataGUI(this.root, this.asset);

            return this.root;
        }

        private L10nAssetElement CreateDataGUI(VisualElement root, L10nAsset asset)
        {
            var assetElem = new L10nAssetElement {
                text = $"Localization ({asset.L10nTexts.Count})",
                value = false,
            };

            var theme = EditorGUIUtility.isProSkin ? "Dark" : "Light";

            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(L10nAsset)}/Layout"));
            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(L10nAsset)}/{theme}Theme"));

            foreach (var kv in asset.L10nTexts)
            {
                CreateLocalTextGUI(assetElem, asset, kv.Value);
            }

            root.Add(assetElem);
            return assetElem;
        }

        private L10nTextElement CreateLocalTextGUI(VisualElement root, L10nAsset asset, L10nTextRow text)
        {
            var contentElems = new List<ContentElement>();
            var textElem = new L10nTextElement {
                userData = contentElems,
                text = text.Id
            };

            var languages = new List<string>(asset.Languages);
            var languagesPopup = new LanguagePopup("Language Id", languages, 0);
            languagesPopup.RegisterValueChangedCallback(OnChangeLanguage);
            textElem.Content.Add(languagesPopup);

            CreateContentGUI(textElem.Content, asset, text, contentElems, languages[0]);

            root.Add(textElem);
            return textElem;
        }

        private ContentElement CreateContentGUI(VisualElement root, L10nAsset asset, L10nTextRow text,
                                                IList<ContentElement> contentElems, string language)
        {
            root.userData = asset;
            var contentText = asset.GetContent(text.ContentId)?.GetLocalization(language);

            if (string.IsNullOrEmpty(contentText))
            {
                return null;
            }

            var contentElem = new ContentElement {
                userData = text.ContentId,
                value = contentText,
                label = "Text"
            };

            contentElems.Add(contentElem);
            root.Add(contentElem);
            return contentElem;
        }

        private void OnChangeLanguage(ChangeEvent<string> evt)
        {
            var contentsContainer = (evt.currentTarget as VisualElement).parent.parent;

            if (!(contentsContainer.userData is IList<ContentElement> contents))
                return;

            L10nAsset asset = null;

            foreach (var content in contents)
            {
                if (!ReferenceEquals(asset, content.parent.userData))
                {
                    asset = content.parent.userData as L10nAsset;
                }

                var id = (int)content.userData;
                content.value = asset.GetContent(id)?.GetLocalization(evt.newValue) ?? string.Empty;
            }
        }
    }
}