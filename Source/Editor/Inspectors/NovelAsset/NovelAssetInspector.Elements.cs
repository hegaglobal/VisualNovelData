using UnityEngine.UIElements;

namespace VisualNovelData.Data.Editor
{
    public partial class NovelAssetInspector
    {
        private class NovelAssetElement : Folder
        {
            public NovelAssetElement() : base("novel-asset")
            {
            }
        }

        private class ConversationElement : Folder
        {
            public ConversationElement() : base("conversation")
            {
                this.value = false;
            }
        }

        private class DialogueElement : Folder
        {
            public DialogueElement() : base("dialogue")
            {
            }
        }

        private class IdElement : ReadOnlyTextField
        {
            public IdElement()
            {
                AddToClassList("id");
            }
        }

        private class ActorElement : ReadOnlyTextField
        {
            public ActorElement(string label) : base(label)
            {
                AddToClassList("actor");
            }
        }

        private class ActionElement : ReadOnlyTextField
        {
            public ActionElement(string label) : base(label)
            {
                AddToClassList("action");
            }
        }

        private class ChoiceContainer : Container
        {
            public ChoiceContainer() : base("choice")
            {
            }
        }

        private class DefaultChoiceElement : VisualElement
        {
            public DefaultChoiceElement()
            {
                AddToClassList("default-choice");
            }
        }

        private class ChoiceElement : Folder
        {
            public ChoiceElement() : base("choice")
            {
                this.value = false;
            }
        }

        private class GoToElement : ReadOnlyTextField
        {
            public GoToElement()
            {
                AddToClassList("go-to");
            }

            public GoToElement(string label) : base(label)
            {
                AddToClassList("go-to");
            }
        }

        private class EventTriggerContainer : Container
        {
            public EventTriggerContainer() : base("event-trigger")
            {
            }
        }

        private class TriggerContainer : Container
        {
            public TriggerContainer() : base("trigger")
            {
            }
        }

        private abstract class TriggerElement : VisualElement
        {
            public TriggerElement()
            {
                AddToClassList("on-trigger");
            }
        }

        private class OnStartElement : TriggerElement
        {
            public OnStartElement()
            {
                AddToClassList("on-start");
            }
        }

        private class OnEndElement : TriggerElement
        {
            public OnEndElement()
            {
                AddToClassList("on-end");
            }
        }

        private class EventContainer : Container
        {
            public EventContainer() : base("event")
            {
            }
        }

        private class EventElement : ReadOnlyTextField
        {
            public EventElement() : base(string.Empty)
            {
                AddToClassList("event");
            }
        }

        private class EventsOnStartElement : ReadOnlyTextField
        {
            public EventsOnStartElement(string label) : base(label)
            {
                AddToClassList("events-on-start");
                this.multiline = true;
            }
        }

        private class EventsOnEndElement : ReadOnlyTextField
        {
            public EventsOnEndElement(string label) : base(label)
            {
                AddToClassList("events-on-end");
                this.multiline = true;
            }
        }
    }
}