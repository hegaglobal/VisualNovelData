using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace VisualNovelData.Data.Editor
{
    [CustomEditor(typeof(QuestAsset), true)]
    public partial class QuestAssetInspector : UnityEditor.Editor
    {
        protected VisualElement root;
        protected QuestAsset asset;

        private void OnEnable()
        {
            this.asset = this.target as QuestAsset;
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.root = new VisualElement();
            CreateDataGUI(this.root, this.asset);

            return this.root;
        }

        private QuestAssetElement CreateDataGUI(VisualElement root, QuestAsset asset)
        {
            var assetElem = new QuestAssetElement {
                text = $"Quest ({asset.Quests.Count})",
                value = false,
            };

            var theme = EditorGUIUtility.isProSkin ? "Dark" : "Light";

            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(QuestAsset)}/Layout"));
            assetElem.styleSheets.Add(Resources.Load<StyleSheet>($"{nameof(QuestAsset)}/{theme}Theme"));

            foreach (var kv in asset.Quests)
            {
                CreateQuestGUI(assetElem, kv.Value);
            }

            root.Add(assetElem);
            return assetElem;
        }

        private QuestElement CreateQuestGUI(VisualElement root, QuestRow quest)
        {
            var questElem = new QuestElement { text = quest.Id };

            var progressContainer = new ProgressContainer();
            questElem.Content.Add(new ProgressTypeElement { value = quest.ProgressType.ToKeywordString() });
            questElem.Content.Add(progressContainer);

            foreach (var stage in quest.Progress)
            {
                var stageContainer = new StageContainer();
                progressContainer.Add(stageContainer);

                stageContainer.Add(new StageElement { value = $"{stage.Index}" });

                if (stage.MaxConstraint >= 0)
                {
                    stageContainer.Add(new StageElement { value = $"<{stage.MaxConstraint}>" });
                }

                var eventContainer = new EventContainer();
                stageContainer.Add(eventContainer);

                foreach (var e in stage.Events)
                {
                    eventContainer.Add(new EventElement { value = e.Id });
                }
            }

            root.Add(questElem);
            return questElem;
        }
    }
}