using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace VisualNovelData.Data.Editor
{
    [CustomEditor(typeof(NovelAsset), true)]
    public partial class NovelAssetInspector : UnityEditor.Editor
    {
        protected VisualElement root;
        protected NovelAsset asset;

        private void OnEnable()
        {
            this.asset = this.target as NovelAsset;
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.root = new VisualElement();
            CreateDataGUI(this.root, this.asset);

            return this.root;
        }

        private NovelAssetElement CreateDataGUI(VisualElement root, NovelAsset asset)
        {
            var assetElem = new NovelAssetElement {
                text = $"Conversations ({asset.Conversations.Count})",
                value = false,
            };

            var theme = EditorGUIUtility.isProSkin ? "Dark" : "Light";

            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(NovelAsset)}/Layout"));
            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(NovelAsset)}/{theme}Theme"));

            foreach (var kv in asset.Conversations)
            {
                CreateConversationGUI(assetElem, asset, kv.Value);
            }

            root.Add(assetElem);
            return assetElem;
        }

        private ConversationElement CreateConversationGUI(VisualElement root, NovelAsset asset,
                                                          ConversationRow conversation)
        {
            var contentElems = new List<ContentElement>();
            var conversationElem = new ConversationElement {
                userData = contentElems,
                text = conversation.Id
            };

            var languages = new List<string>(asset.Languages);
            var languagesPopup = new LanguagePopup("Language Id", languages, 0);
            languagesPopup.RegisterValueChangedCallback(OnChangeLanguage);
            conversationElem.Add(languagesPopup);

            foreach (var kv in conversation.Dialogues)
            {
                CreateDialogueGUI(conversationElem.Content, conversation, kv.Value, contentElems, languages[0]);
            }

            root.Add(conversationElem);
            return conversationElem;
        }

        private DialogueElement CreateDialogueGUI(VisualElement root, ConversationRow conversation,
                                                  DialogueRow dialogue, IList<ContentElement> contents, string language)
        {
            var dialogueElem = new DialogueElement();

            var idElem = new IdElement { value = dialogue.Id };
            dialogueElem.Header.Add(idElem);

            var defaultChoiceData = dialogue.GetChoice(0);

            if (defaultChoiceData != null)
            {
                CreateDefaultChoiceGUI(dialogueElem, conversation, defaultChoiceData, contents, language);
            }

            if (dialogue.Choices.Count > 1)
            {
                var choiceContainer = new ChoiceContainer();
                choiceContainer.Add(new Label("Choices"));

                var first = true;

                foreach (var kv in dialogue.Choices)
                {
                    if (kv.Value.Id == 0)
                        continue;

                    var choiceElem = CreateChoiceGUI(choiceContainer, conversation, kv.Value, contents, language);

                    if (first)
                    {
                        first = false;
                        choiceElem.style.marginTop = 0;
                    }
                }

                dialogueElem.Add(choiceContainer);
            }

            if (!dialogue.IsEnd())
            {
                var sb = new StringBuilder();
                var delayElem = new DelayElement("Delay") { value = dialogue.Delay.ToString() };
                dialogueElem.Add(delayElem);

                var speakerElem = new SpeakerElement("Speaker") { value = dialogue.Speaker };
                dialogueElem.Add(speakerElem);

                if (HasActors(dialogue))
                {
                    CreateCharacterTableGUI(dialogueElem, out var columnContainerElem);

                    CreateCharacterList<ActorColumnElement>(columnContainerElem, "Actor",
                                                            dialogue.Actor1, dialogue.Actor2, dialogue.Actor3, dialogue.Actor4);

                    CreateCharacterList<ActionColumnElement>(columnContainerElem, "Actions",
                                                             dialogue.Actions1.BuildString(sb), dialogue.Actions2.BuildString(sb),
                                                             dialogue.Actions3.BuildString(sb), dialogue.Actions4.BuildString(sb));
                }

                var highlightElem = new HighlightElement("Highlight") { value = dialogue.Highlight.BuildString(sb) };
                dialogueElem.Add(highlightElem);
            }

            if (dialogue.CommandsOnStart.Count > 0 ||
                dialogue.CommandsOnEnd.Count > 0)
            {
                CreateCommandTableGUI(dialogueElem, dialogue.CommandsOnStart, dialogue.CommandsOnEnd);
            }

            root.Add(dialogueElem);
            return dialogueElem;
        }

        private bool HasActors(DialogueRow dialogue)
            => !(string.IsNullOrEmpty(dialogue.Actor1) && string.IsNullOrEmpty(dialogue.Actor2) &&
                 string.IsNullOrEmpty(dialogue.Actor3) && string.IsNullOrEmpty(dialogue.Actor4));

        private DefaultChoiceElement CreateDefaultChoiceGUI(DialogueElement root, ConversationRow conversation,
                                                            ChoiceRow choice, IList<ContentElement> contents, string language)
        {
            var choiceElem = new DefaultChoiceElement();
            CreateChoiceContent(root.Header, choiceElem, conversation, choice, contents, language);

            root.Add(choiceElem);
            return choiceElem;
        }

        private ChoiceElement CreateChoiceGUI(VisualElement root, ConversationRow conversation,
                                              ChoiceRow choice, IList<ContentElement> contents, string language)
        {
            var choiceElem = new ChoiceElement();

            var idElem = new IdElement { value = choice.Id.ToString() };
            choiceElem.Header.Add(idElem);

            CreateChoiceContent(choiceElem.Header, choiceElem.Content, conversation, choice, contents, language);

            root.Add(choiceElem);
            return choiceElem;
        }

        private void CreateChoiceContent(VisualElement header, VisualElement content, ConversationRow conversation,
                                         ChoiceRow choice, IList<ContentElement> contents, string language)
        {
            CreateContentGUI(header, conversation, choice, contents, language);
            CreateGoToGUI(content, choice);
        }

        private ContentElement CreateContentGUI(VisualElement root, ConversationRow conversation,
                                                ChoiceRow choice, IList<ContentElement> contentElems, string language)
        {
            root.userData = conversation;
            var contentText = conversation.GetContent(choice.ContentId)?.GetLocalization(language);

            if (string.IsNullOrEmpty(contentText))
            {
                return null;
            }

            var contentElem = new ContentElement {
                userData = choice.ContentId,
                value = contentText
            };

            contentElems.Add(contentElem);
            root.Add(contentElem);
            return contentElem;
        }

        private GoToElement CreateGoToGUI(VisualElement root, ChoiceRow choice)
        {
            if (string.IsNullOrEmpty(choice.GoTo))
                return null;

            var goToElem = new GoToElement("Go To") { value = choice.GoTo };
            root.Add(goToElem);
            return goToElem;
        }

        private TableContainer CreateCharacterTableGUI(VisualElement root, out ColumnContainer columnContainer)
        {
            var tableContainer = new TableContainer();
            root.Add(tableContainer);

            tableContainer.Add(new Label("Characters"));

            columnContainer = new ColumnContainer();
            tableContainer.Add(columnContainer);

            return tableContainer;
        }

        private T CreateCharacterList<T>(VisualElement root, string label, params string[] characters)
            where T : ColumnElement, new()
        {
            var columnElem = new T();
            root.Add(columnElem);

            columnElem.Add(new Label(label));

            if (characters.Length > 0)
            {
                var rowContainer = new RowContainer();
                columnElem.Add(rowContainer);

                for (var i = 0; i < characters.Length; i++)
                {
                    var rowElem = new RowElement { value = characters[i] };
                    columnElem.Add(rowElem);
                }
            }

            return columnElem;
        }

        private TableContainer CreateCommandTableGUI(VisualElement root, ICommandList onStart, ICommandList onEnd)
        {
            var tableContainer = new TableContainer();
            root.Add(tableContainer);

            tableContainer.Add(new Label("Commands"));

            var columnContainer = new ColumnContainer();
            tableContainer.Add(columnContainer);

            CreateCommandList<OnStartColumnElement>(columnContainer, "On Start", onStart);
            CreateCommandList<OnEndColumnElement>(columnContainer, "On End", onEnd);

            return tableContainer;
        }

        private T CreateCommandList<T>(VisualElement root, string label, IReadOnlyList<Command> commands)
            where T : ColumnElement, new()
        {
            var columnElem = new T();
            root.Add(columnElem);

            columnElem.Add(new Label(label));

            if (commands.Count > 0)
            {
                var rowContainer = new RowContainer();
                columnElem.Add(rowContainer);

                for (var i = 0; i < commands.Count; i++)
                {
                    var rowElem = new RowElement { value = commands[i].Id };
                    columnElem.Add(rowElem);
                }
            }

            return columnElem;
        }

        private void OnChangeLanguage(ChangeEvent<string> evt)
        {
            var contentsContainer = (evt.currentTarget as VisualElement).parent;

            if (!(contentsContainer.userData is List<ContentElement> contents))
                return;

            ConversationRow conversation = null;

            foreach (var content in contents)
            {
                if (!ReferenceEquals(conversation, content.parent.userData))
                {
                    conversation = content.parent.userData as ConversationRow;
                }

                var id = (int)content.userData;
                content.value = conversation?.GetContent(id)?.GetLocalization(evt.newValue) ?? string.Empty;
            }
        }
    }

    internal static class CollectionExtensions
    {
        public static string BuildString(this IReadOnlyList<Command> self, StringBuilder sb)
        {
            sb.Clear();

            for (var i = 0; i < self.Count; i++)
            {
                if (self[i] == null)
                    continue;

                if (i > 0)
                    sb.Append(" | ");

                sb.Append($"{self[i].Id}");
            }

            return sb.ToString();
        }

        public static string BuildString<T>(in this ReadArray1<T> self, StringBuilder sb)
        {
            sb.Clear();

            for (var i = 0; i < self.Length; i++)
            {
                if (self[i] == null)
                    continue;

                if (i > 0)
                    sb.Append(", ");

                sb.Append($"{self[i]}");
            }

            return sb.ToString();
        }
    }
}