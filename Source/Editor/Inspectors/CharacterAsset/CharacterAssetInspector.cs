using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace VisualNovelData.Data.Editor
{
    [CustomEditor(typeof(CharacterAsset), true)]
    public partial class CharacterAssetInspector : UnityEditor.Editor
    {
        protected VisualElement root;
        protected CharacterAsset asset;

        private void OnEnable()
        {
            this.asset = this.target as CharacterAsset;
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.root = new VisualElement();
            CreateDataGUI(this.root, this.asset);

            return this.root;
        }

        private CharacterAssetElement CreateDataGUI(VisualElement root, CharacterAsset asset)
        {
            var assetElem = new CharacterAssetElement {
                text = $"Characters ({asset.Characters.Count})",
                value = false,
            };

            var theme = EditorGUIUtility.isProSkin ? "Dark" : "Light";

            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(CharacterAsset)}/Layout"));
            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(CharacterAsset)}/{theme}Theme"));

            foreach (var kv in asset.Characters)
            {
                CreateCharacterGUI(assetElem, asset, kv.Value);
            }

            root.Add(assetElem);
            return assetElem;
        }

        private CharacterElement CreateCharacterGUI(VisualElement root, CharacterAsset asset, CharacterRow character)
        {
            var contentElems = new List<ContentElement>();
            var characterElem = new CharacterElement {
                userData = contentElems,
                text = character.Id.Or("<none>")
            };

            var languages = new List<string>(asset.Languages);
            var languagesPopup = new LanguagePopup("Language Id", languages, 0);
            languagesPopup.RegisterValueChangedCallback(OnChangeLanguage);
            characterElem.Content.Add(languagesPopup);

            CreateAvatarGUI(characterElem.Content, character);
            CreateModelGUI(characterElem.Content, character);
            CreateBackgroundGUI(characterElem.Content, character);
            CreateContentGUI(characterElem.Content, asset, character, contentElems, languages[0]);

            root.Add(characterElem);
            return characterElem;
        }

        private AvatarElement CreateAvatarGUI(VisualElement root, CharacterRow character)
        {
            var avatarElem = new AvatarElement("Avatar") { value = character.Avatar };
            root.Add(avatarElem);

            return avatarElem;
        }

        private ModelElement CreateModelGUI(VisualElement root, CharacterRow character)
        {
            var modelElem = new ModelElement("Model") { value = character.Model };
            root.Add(modelElem);

            return modelElem;
        }

        private BackgroundElement CreateBackgroundGUI(VisualElement root, CharacterRow character)
        {
            var backgroundElem = new BackgroundElement("Background") { value = character.Background };
            root.Add(backgroundElem);

            return backgroundElem;
        }

        private ContentElement CreateContentGUI(VisualElement root, CharacterAsset asset, CharacterRow character,
                                                IList<ContentElement> contentElems, string language)
        {
            root.userData = asset;
            var contentText = asset.GetContent(character.ContentId)?.GetLocalization(language);

            if (string.IsNullOrEmpty(contentText))
            {
                return null;
            }

            var contentElem = new ContentElement {
                userData = character.ContentId,
                value = contentText,
                label = "Content"
            };

            contentElems.Add(contentElem);
            root.Add(contentElem);
            return contentElem;
        }

        private void OnChangeLanguage(ChangeEvent<string> evt)
        {
            var contentsContainer = (evt.currentTarget as VisualElement).parent.parent;

            if (!(contentsContainer.userData is List<ContentElement> contents))
                return;

            CharacterAsset asset = null;

            foreach (var content in contents)
            {
                if (!ReferenceEquals(asset, content.parent.userData))
                {
                    asset = content.parent.userData as CharacterAsset;
                }

                var id = (int)content.userData;
                content.value = asset.GetContent(id)?.GetLocalization(evt.newValue) ?? string.Empty;
            }
        }
    }
}