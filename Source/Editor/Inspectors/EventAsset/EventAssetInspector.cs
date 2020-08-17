using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace VisualNovelData.Data.Editor
{
    [CustomEditor(typeof(EventAsset), true)]
    public partial class EventAssetInspector : UnityEditor.Editor
    {
        protected VisualElement root;
        protected EventAsset asset;

        private void OnEnable()
        {
            this.asset = this.target as EventAsset;
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.root = new VisualElement();
            CreateDataGUI(this.root, this.asset);

            return this.root;
        }

        private EventAssetElement CreateDataGUI(VisualElement root, EventAsset asset)
        {
            var assetElem = new EventAssetElement {
                text = $"Event ({asset.Events.Count})",
                value = false,
            };

            var theme = EditorGUIUtility.isProSkin ? "Dark" : "Light";

            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(EventAsset)}/Layout"));
            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(EventAsset)}/{theme}Theme"));

            foreach (var kv in asset.Events)
            {
                CreateEventGUI(assetElem, kv.Value);
            }

            root.Add(assetElem);
            return assetElem;
        }

        private EventElement CreateEventGUI(VisualElement root, EventRow @event)
        {
            var eventElem = new EventElement { text = @event.Id };

            var stagesContainer = new StagesContainer();
            eventElem.Content.Add(new InvokeTypeElement { value = @event.InvokeType.ToKeyword() });
            eventElem.Content.Add(stagesContainer);

            foreach (var stage in @event.Stages)
            {
                var stageContainer = new StageContainer();
                stagesContainer.Add(stageContainer);

                stageContainer.Add(new StageElement { value = $"{stage.Index}" });

                if (stage.MaxConstraint >= 0)
                {
                    stageContainer.Add(new StageElement { value = $"<{stage.MaxConstraint}>" });
                }

                var commandContainer = new CommandContainer();
                stageContainer.Add(commandContainer);

                foreach (var command in stage.Commands)
                {
                    commandContainer.Add(new CommandElement { value = command.Id });
                }
            }

            root.Add(eventElem);
            return eventElem;
        }
    }
}