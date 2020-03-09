using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VisualNovelData.Data.Editor
{
    public class Folder : Foldout
    {
        public VisualElement Header { get; }

        public VisualElement Content { get; }

        public Folder(string prefix)
        {
            AddToClassList($"{prefix}-folder");

            this.Header = this.Q<Toggle>().Q(className: "unity-toggle__input");
            this.Header?.AddToClassList($"{prefix}-header");

            this.Content = this.Q("unity-content");
            this.Content?.AddToClassList($"{prefix}-content");
        }
    }

    public class Container : VisualElement
    {
        public Container(string prefix)
        {
            AddToClassList($"{prefix}-container");
        }
    }

    public class ReadOnlyTextField : TextField
    {
        public ReadOnlyTextField()
        {
        }

        public ReadOnlyTextField(string label) : base(label)
        {
            this.isReadOnly = true;
        }
    }

    public class LanguagePopup : PopupField<string>
    {
        public LanguagePopup(string label, List<string> choices, int defaultIndex) : base(label, choices, defaultIndex)
        {
            AddToClassList("language-popup");
        }
    }

    public class ContentElement : ReadOnlyTextField
    {
        public ContentElement()
        {
            AddToClassList("content");
            this.multiline = true;
        }

        public ContentElement(string label) : base(label)
        {
            AddToClassList("content");
            this.multiline = true;
        }
    }
}