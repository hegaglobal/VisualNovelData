using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using VisualNovelData.Data;

namespace VisualNovelData.Parser
{
    public class EventParser
    {
        private static readonly char[] _eventSeparators = new[] { '[', ']' };
        private static readonly string[] _paramSeparators = new[] { "::" };
        private static readonly Regex _eventConstraintRegex = new Regex(@"\<(?<constraint>\d*)\>(?<type>[^\n\r]*)", RegexOptions.Compiled);
        private static readonly Regex _positionalParamRegex = new Regex(@"^{(?<position>\d+)}=(?<value>[^\n\r]*)$", RegexOptions.Compiled);
        private static readonly string[] _noParam = new string[0];

        private readonly StringBuilder errorLogger = new StringBuilder();
        private readonly StringBuilder idBuilder = new StringBuilder();
        private readonly List<Parameter> parameters = new List<Parameter>();

        public IReadOnlyList<Event> Parse(string eventsString, StringBuilder errorLogger)
        {
            this.errorLogger.Clear();

            var events = eventsString?.Trim()?.Split(_eventSeparators, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<Event>();

            for (var i = 0; i < events.Length; i++)
            {
                var parts = events[i].Split(_paramSeparators, StringSplitOptions.RemoveEmptyEntries);
                var @event = ToEvent(parts);

                if (this.errorLogger.Length > 0)
                {
                    errorLogger.AppendLine(this.errorLogger.ToString());
                    return null;
                }

                if (@event != null)
                    list.Add(@event);
            }

            return list;
        }

        private Event ToEvent(in Segment<string> parts)
        {
            if (parts.Count <= 0)
                return null;

            this.idBuilder.Clear();
            var eventDef = parts[0];

            if (string.IsNullOrEmpty(eventDef))
            {
                this.errorLogger.AppendLine("Event type is empty");
                return null;
            }

            var match = _eventConstraintRegex.Match(eventDef);
            string eventType;
            var maxConstraint = -1;

            if (!match.Success)
            {
                eventType = eventDef;
            }
            else if (int.TryParse(match.Groups["constraint"].Value, out var constraint))
            {
                maxConstraint = constraint;
                eventType = match.Groups["type"].Value;
            }
            else
            {
                this.errorLogger.AppendLine($"Cannot convert max constraint to integer at `{eventDef}`");
                return null;
            }

            if (maxConstraint >= 0)
                this.idBuilder.Append($"<{maxConstraint}> ");

            this.idBuilder.Append(eventType);
            var parameters = ParseParameters(eventDef, eventType, parts.Slice(1));

            if (this.errorLogger.Length > 0)
                return null;

            return new Event(this.idBuilder.ToString(), eventType, maxConstraint, parameters);
        }

        private string[] ParseParameters(string eventDef, string eventName, in Segment<string> parts)
        {
            if (parts.Count < 1)
                return _noParam;

            if (!TryParseParameters(eventDef, eventName, parts))
                return _noParam;

            if (this.parameters.Count <= 0)
                return _noParam;

            if (!TryGetParamaterCount(out var count))
                return _noParam;

            var @params = new string[count];
            this.idBuilder.Append(" (");

            try
            {
                for (var i = 0; i < this.parameters.Count; i++)
                {
                    var param = this.parameters[i];
                    @params[param.position] = param.value;
                }
            }
            catch
            {
                for (var i = 0; i < this.parameters.Count; i++)
                {
                    UnityEngine.Debug.Log($"{this.parameters[i].position} = {this.parameters[i].value}");
                }

                this.errorLogger.AppendLine("index out of bound");
                return _noParam;
            }

            for (var i = 0; i < @params.Length; i++)
            {
                if (string.IsNullOrEmpty(@params[i]))
                    @params[i] = string.Empty;

                if (i == 0)
                    this.idBuilder.Append(@params[i]);
                else
                    this.idBuilder.Append($", {@params[i]}");
            }

            this.idBuilder.Append(')');
            return @params;
        }

        private bool TryGetParamaterCount(out int count)
        {
            var max = 0;

            for (var i = 0; i < this.parameters.Count; i++)
            {
                if (max < this.parameters[i].position)
                    max = this.parameters[i].position;
            }

            count = max + 1;
            return true;
        }

        private bool TryParseParameters(string eventDef, string eventName, in Segment<string> parts)
        {
            this.parameters.Clear();

            for (var i = 0; i < parts.Count; i++)
            {
                var match = _positionalParamRegex.Match(parts[i]);

                if (!match.Success)
                {
                    if (!TryAddParameter(eventName, new Parameter(i, parts[i])))
                        return false;

                    continue;
                }

                var positionStr = match.Groups["position"].Value;

                if (!int.TryParse(positionStr, out var position))
                {
                    this.errorLogger.AppendLine($"Cannot convert position `{positionStr}` to integer at `{eventDef}`");
                    return false;
                }

                if (position <= 0)
                {
                    this.errorLogger.AppendLine($"Position must be greater than 0. Current value: {position}");
                    return false;
                }

                var parameter = new Parameter(position - 1, match.Groups["value"].Value);

                if (!TryAddParameter(eventName, parameter))
                    return false;
            }

            this.parameters.Sort();
            return true;
        }

        private bool TryAddParameter(string eventName, in Parameter parameter)
        {
            if (this.parameters.Contains(parameter))
            {
                this.errorLogger.AppendLine($"Event {eventName} can only have 1 parameter at position {parameter.position}");
                return false;
            }

            this.parameters.Add(parameter);
            return true;
        }

        private readonly struct Parameter : IEquatable<Parameter>, IComparable<Parameter>
        {
            public readonly int position;
            public readonly string value;

            public Parameter(int position, string value)
            {
                this.position = position;
                this.value = value;
            }

            public int CompareTo(Parameter other)
                => this.position.CompareTo(other.position);

            public bool Equals(Parameter other)
                => this.position == other.position;
        }
    }
}