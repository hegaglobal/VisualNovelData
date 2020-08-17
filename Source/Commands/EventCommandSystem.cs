using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Commands
{
    using Data;

    public sealed class EventCommandSystem
    {
        public ReadEventData EventData { get; private set; }

        public CommandSystem CommandSystem { get; private set; }

        public EventCommandSystem() { }

        public EventCommandSystem(CommandSystem commandSystem)
            => Set(commandSystem);

        public EventCommandSystem(in ReadEventData eventData)
            => Set(eventData);

        public EventCommandSystem(in ReadEventData eventData, CommandSystem commandSystem)
        {
            Set(eventData);
            Set(commandSystem);
        }

        public void Set(in ReadEventData eventData)
            => this.EventData = eventData;

        public void Set(CommandSystem commandSystem)
            => this.CommandSystem = commandSystem ?? throw new ArgumentNullException(nameof(commandSystem));

        private bool TryGetEvent(string id, out EventRow @event)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            @event = default;

            if (!this.EventData.HasData)
            {
                Debug.LogWarning($"Cannot get event. No {nameof(EventAsset)} is assigned");
                return false;
            }

            @event = this.EventData.GetEvent(id);

            if (@event == null)
            {
                Debug.LogWarning($"Cannot find any event by id={id}");
                return false;
            }

            return true;
        }

        public void Invoke(string eventId, int stageId)
        {
            if (!TryGetEvent(eventId, out var @event))
                return;

            Segment<StageRow> stages;

            switch (@event.InvokeType)
            {
                case EventRow.EventInvokeType.All:
                    stages = @event.GetStages();
                    break;

                case EventRow.EventInvokeType.StartToCurrent:
                    stages = @event.GetStagesToCurrent(stageId);
                    break;

                case EventRow.EventInvokeType.OnlyCurrent:
                    stages = @event.GetCurrentStage(stageId).AsSegment1();
                    break;

                default:
                    stages = @event.GetEmptyStages();
                    break;
            }

            foreach (var stage in stages)
            {
                if (CanSkip(stage, stageId))
                    continue;

                Invoke(stage.Commands.AsSegment(), stageId);
            }
        }

        private void Invoke(in Segment<Command> commands, int stage)
        {
            if (commands.Count <= 0)
                return;

            if (this.CommandSystem == null)
            {
                Debug.LogWarning($"Cannot invoke commands. No {nameof(this.CommandSystem)} is assigned");
                return;
            }

            this.CommandSystem.Invoke(commands, stage);
        }

        private static bool CanSkip(StageRow stage, int stageId)
            => stage == null ||
               (stage.MaxConstraint >= 0 && stageId > stage.MaxConstraint);
    }
}