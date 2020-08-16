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

        private class DelayElement : ReadOnlyTextField
        {
            public DelayElement(string label) : base(label)
            {
                AddToClassList("delay");
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

        private class HighlightElement : ReadOnlyTextField
        {
            public HighlightElement(string label) : base(label)
            {
                AddToClassList("highlight");
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

        private class TableContainer : Container
        {
            public TableContainer() : base("table")
            {
            }
        }

        private class ColumnContainer : Container
        {
            public ColumnContainer() : base("column")
            {
            }
        }

        private class ColumnElement : VisualElement
        {
            public ColumnElement(string customClass)
            {
                AddToClassList("column");
                AddToClassList(customClass);
            }
        }

        private class RowContainer : Container
        {
            public RowContainer() : base("row")
            {
            }
        }

        private class RowElement : ReadOnlyTextField
        {
            public RowElement() : base(string.Empty)
            {
                AddToClassList("row");
            }
        }

        private class ActorColumnElement : ColumnElement
        {
            public ActorColumnElement() : base("actor")
            {
            }
        }

        private class ActionColumnElement : ColumnElement
        {
            public ActionColumnElement() : base("action")
            {
            }
        }

        private class OnStartColumnElement : ColumnElement
        {
            public OnStartColumnElement() : base("commands-on-start")
            {
            }
        }

        private class OnEndColumnElement : ColumnElement
        {
            public OnEndColumnElement() : base("commands-on-end")
            {
            }
        }
    }
}