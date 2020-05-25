using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

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


            if (!dialogue.IsEnd())
            {
                var delayElem = new DelayElement("Delay") { value = dialogue.Delay.ToString() };
                dialogueElem.Add(delayElem);

                var actorElem = new ActorElement("Actor") { value = dialogue.Actor };
                dialogueElem.Add(actorElem);

                var actionElem = new ActionElement("Action") { value = dialogue.Action };
                dialogueElem.Add(actionElem);

                var highlightElem = new HighlightElement("Highlight") { value = dialogue.Highlight.ToString() };
                dialogueElem.Add(highlightElem);
            }

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

            if (dialogue.EventsOnStart.Count > 0 ||
                dialogue.EventsOnEnd.Count > 0)
            {
                CreateEventsGUI(dialogueElem, dialogue.EventsOnStart, dialogue.EventsOnEnd);
            }

            root.Add(dialogueElem);
            return dialogueElem;
        }

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

        private EventTriggerContainer CreateEventsGUI(VisualElement root, IEventList onStart, IEventList onEnd)
        {
            var eventTriggerContainer = new EventTriggerContainer();
            root.Add(eventTriggerContainer);

            eventTriggerContainer.Add(new Label("Events"));

            var triggerContainer = new TriggerContainer();
            eventTriggerContainer.Add(triggerContainer);

            CreateOnEventList<OnStartElement>(triggerContainer, "On Start", onStart);
            CreateOnEventList<OnEndElement>(triggerContainer, "On End", onEnd);

            return eventTriggerContainer;
        }

        private T CreateOnEventList<T>(VisualElement root, string label, IReadOnlyList<Event> eventList)
            where T : TriggerElement, new()
        {
            var triggerElem = new T();
            root.Add(triggerElem);

            triggerElem.Add(new Label(label));

            if (eventList.Count > 0)
            {
                var eventContainer = new EventContainer();
                triggerElem.Add(eventContainer);

                for (var i = 0; i < eventList.Count; i++)
                {
                    var eventElem = new EventElement { value = eventList[i].Id };
                    triggerElem.Add(eventElem);
                }
            }

            return triggerElem;
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
}