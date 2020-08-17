using System.Text.RegularExpressions;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public partial class VseRow
    {
        private static readonly Regex _digitsRegex = new Regex(@"\d+", RegexOptions.Compiled);

        public override bool IsEmpty
            => !(this.HasEvent || this.HasStage || this.HasCommands);

        public bool IsEventStart
            => this.HasEvent && !this.Event.StartsWith('[') && !this.Event.EndsWith(']') &&
               !(this.HasStage || this.HasCommands);

        public bool IsEventEnd
            => this.HasEvent && this.Event.StartsWith('[') && this.Event.EndsWith(']') &&
               !(this.HasStage || this.HasCommands);

        public bool HasEvent
            => !string.IsNullOrEmpty(this.Event);

        public bool HasStage
            => !string.IsNullOrEmpty(this.Stage) && _digitsRegex.IsMatch(this.Stage);

        public bool HasCommands
            => !string.IsNullOrEmpty(this.Commands);

        public override string ToString()
        {
            this.stringBuilder.Clear();
            this.stringBuilder.Append($"{this.Event} - {this.Stage} - {this.Commands}");

            return this.stringBuilder.ToString();
        }

        public EventRow Parse(IEventData data, EventRow @event, CommandParser commandParser, int row)
        {
            this.error.Clear();

            if (!this.IsEmpty)
            {
                if (this.IsEventStart)
                    return ParseEventStart(data, @event, row);

                if (this.IsEventEnd)
                    return ParseEventEnd(@event, data);

                if (this.HasStage)
                    ParseStage(@event, commandParser);
            }

            return @event;
        }

        private EventRow ParseEventStart(IEventData data, EventRow @event, int row)
        {
            var id = this.Event;

            if (!this.idRegex.IsMatch(id))
            {
                this.error.AppendLine($"Event id must only contain characters in [a-zA-Z0-9_]. Current value: {this.Event}");
                return null;
            }

            if (@event != null && !@event.Id.Equals(id))
            {
                this.error.AppendLine($"Event `{@event.Id}` must end before starting a new one");
                return @event;
            }

            if (!this.Stage.TryParse(out var invokeType))
            {
                this.error.AppendLine(string.Format("Invoke type can only be either empty, {0}, {1}, or {2}",
                                      InvokeTypeExtensions.ALL,
                                      InvokeTypeExtensions.START_TO_CURRENT,
                                      InvokeTypeExtensions.ONLY_CURRENT));
                return null;
            }

            if (data.Events.ContainsKey(id))
            {
                Debug.LogWarning($"Vsq row {row}: Event id has already existed");
            }

            return new EventRow(row, id, invokeType);
        }

        private EventRow ParseEventEnd(EventRow @event, IEventData data)
        {
            if (@event == null)
            {
                this.error.AppendLine("No event");
                return @event;
            }

            if (string.IsNullOrEmpty(@event.Id))
            {
                this.error.AppendLine("Invalid event id");
                return @event;
            }

            var start = $"[{@event.Id}]";
            var end = this.Event;

            if (!start.Equals(end))
            {
                this.error.AppendLine("Mismatched event start and end tokens");
                return @event;
            }

            data.AddEvent(@event);
            return null;
        }

        private void ParseStage(EventRow @event, CommandParser commandParser)
        {
            var stage = 0;

            if (this.HasStage &&
                !int.TryParse(this.Stage, out stage))
            {
                this.error.AppendLine($"Cannot convert stage value to integer. Current value: {this.Stage}");
                return;
            }

            if (stage < 0)
            {
                this.error.AppendLine($"State value must be positive (stage >= 0). Current value: {stage}");
                return;
            }

            if (@event.ContainsStage(stage))
            {
                this.error.AppendLine($"Stage {stage} has already existed");
                return;
            }

            var maxConstraint = this.MaxConstraint ?? -1;
            var command = commandParser.Parse(this.Commands, this.error);

            if (this.IsError)
                return;

            @event.AddStage(stage, command, maxConstraint);
        }
    }
}