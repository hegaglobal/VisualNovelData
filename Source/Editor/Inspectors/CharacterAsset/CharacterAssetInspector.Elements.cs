namespace VisualNovelData.Data.Editor
{
    public partial class CharacterAssetInspector
    {
        private class CharacterAssetElement : Folder
        {
            public CharacterAssetElement() : base("character-asset")
            {
            }
        }

        private class CharacterElement : Folder
        {
            public CharacterElement() : base("character")
            {
                this.value = false;
            }
        }

        private class AvatarElement : ReadOnlyTextField
        {
            public AvatarElement(string label) : base(label)
            {
                AddToClassList("avatar");
            }
        }

        private class ModelElement : ReadOnlyTextField
        {
            public ModelElement(string label) : base(label)
            {
                AddToClassList("model");
            }
        }

        private class BackgroundElement : ReadOnlyTextField
        {
            public BackgroundElement(string label) : base(label)
            {
                AddToClassList("background");
            }
        }
    }
}