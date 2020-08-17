namespace VisualNovelData.Data.Editor
{
    public partial class EventAssetInspector
    {
        private class EventAssetElement : Folder
        {
            public EventAssetElement() : base("event-asset")
            {
            }
        }

        private class EventElement : Folder
        {
            public EventElement() : base("event")
            {
                this.value = false;
            }
        }

        private class InvokeTypeElement : ReadOnlyTextField
        {
            public InvokeTypeElement() : base(string.Empty)
            {
                AddToClassList("invoke-type");
            }
        }

        private class StagesContainer : Container
        {
            public StagesContainer() : base("stages")
            {
            }
        }

        private class StageContainer : Container
        {
            public StageContainer() : base("stage")
            {
            }
        }

        private class CommandContainer : Container
        {
            public CommandContainer() : base("command")
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

        private class CommandElement : ReadOnlyTextField
        {
            public CommandElement() : base(string.Empty)
            {
                AddToClassList("command");
            }
        }
    }
}