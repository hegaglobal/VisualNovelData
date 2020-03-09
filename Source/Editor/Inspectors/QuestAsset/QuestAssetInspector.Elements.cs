using UnityEngine.UIElements;

namespace VisualNovelData.Data.Editor
{
    public partial class QuestAssetInspector
    {
        private class QuestAssetElement : Folder
        {
            public QuestAssetElement() : base("quest-asset")
            {
            }
        }

        private class QuestElement : Folder
        {
            public QuestElement() : base("quest")
            {
                this.value = false;
            }
        }

        private class ProgressTypeElement : ReadOnlyTextField
        {
            public ProgressTypeElement() : base(string.Empty)
            {
                AddToClassList("progress-type");
            }
        }

        private class ProgressContainer : Container
        {
            public ProgressContainer() : base("progress")
            {
            }
        }

        private class StageContainer : Container
        {
            public StageContainer() : base("stage")
            {
            }
        }

        private class EventContainer : Container
        {
            public EventContainer() : base("event")
            {
            }
        }

        private class StageElement : ReadOnlyTextField
        {
            public StageElement() : base(string.Empty)
            {
                AddToClassList("stage");
            }
        }

        private class EventElement : ReadOnlyTextField
        {
            public EventElement() : base(string.Empty)
            {
                AddToClassList("event");
            }
        }
    }
}