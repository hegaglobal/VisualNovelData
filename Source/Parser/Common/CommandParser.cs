using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using VisualNovelData.Data;

namespace VisualNovelData.Parser
{
    public class CommandParser
    {
        private static readonly char[] _commandSeparators = new[] { '[', ']' };
        private static readonly string[] _paramSeparators = new[] { "::" };
        private static readonly Regex _commandConstraintRegex = new Regex(@"\<(?<constraint>\d*)\>(?<key>[^\n\r]*)", RegexOptions.Compiled);
        private static readonly Regex _positionalParamRegex = new Regex(@"^{(?<position>\d+)}=(?<value>[^\n\r]*)$", RegexOptions.Compiled);
        private static readonly string[] _noParam = new string[0];

        private readonly StringBuilder errorLogger = new StringBuilder();
        private readonly StringBuilder idBuilder = new StringBuilder();
        private readonly List<Parameter> parameters = new List<Parameter>();

        public IReadOnlyList<Command> Parse(string commandStr, StringBuilder errorLogger)
        {
            this.errorLogger.Clear();

            var commands = commandStr?.Trim()?.Split(_commandSeparators, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<Command>();

            for (var i = 0; i < commands.Length; i++)
            {
                var parts = commands[i].Split(_paramSeparators, StringSplitOptions.RemoveEmptyEntries);
                var command = ToCommand(parts);

                if (this.errorLogger.Length > 0)
                {
                    errorLogger.AppendLine(this.errorLogger.ToString());
                    return null;
                }

                if (command != null)
                    list.Add(command);
            }

            return list;
        }

        private Command ToCommand(in Segment<string> parts)
        {
            if (parts.Count <= 0)
                return null;

            this.idBuilder.Clear();
            var commandDef = parts[0];

            if (string.IsNullOrEmpty(commandDef))
            {
                this.errorLogger.AppendLine("Command type is empty");
                return null;
            }

            var match = _commandConstraintRegex.Match(commandDef);
            var maxConstraint = -1;
            string commandKey;

            if (!match.Success)
            {
                commandKey = commandDef;
            }
            else if (int.TryParse(match.Groups["constraint"].Value, out var constraint))
            {
                maxConstraint = constraint;
                commandKey = match.Groups["key"].Value;
            }
            else
            {
                this.errorLogger.AppendLine($"Cannot convert max constraint to integer at `{commandDef}`");
                return null;
            }

            if (maxConstraint >= 0)
                this.idBuilder.Append($"<{maxConstraint}> ");

            this.idBuilder.Append(commandKey);
            var parameters = ParseParameters(commandDef, commandKey, parts.Slice(1));

            if (this.errorLogger.Length > 0)
                return null;

            return new Command(this.idBuilder.ToString(), commandKey, maxConstraint, parameters);
        }

        private string[] ParseParameters(string commandDef, string commandName, in Segment<string> parts)
        {
            if (parts.Count < 1)
                return _noParam;

            if (!TryParseParameters(commandDef, commandName, parts))
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

        private bool TryParseParameters(string commandDef, string commandName, in Segment<string> parts)
        {
            this.parameters.Clear();

            for (var i = 0; i < parts.Count; i++)
            {
                var match = _positionalParamRegex.Match(parts[i]);

                if (!match.Success)
                {
                    if (!TryAddParameter(commandName, new Parameter(i, parts[i])))
                        return false;

                    continue;
                }

                var positionStr = match.Groups["position"].Value;

                if (!int.TryParse(positionStr, out var position))
                {
                    this.errorLogger.AppendLine($"Cannot convert position `{positionStr}` to integer at `{commandDef}`");
                    return false;
                }

                if (position <= 0)
                {
                    this.errorLogger.AppendLine($"Position must be greater than 0. Current value: {position}");
                    return false;
                }

                var parameter = new Parameter(position - 1, match.Groups["value"].Value);

                if (!TryAddParameter(commandName, parameter))
                    return false;
            }

            this.parameters.Sort();
            return true;
        }

        private bool TryAddParameter(string commandName, in Parameter parameter)
        {
            if (this.parameters.Contains(parameter))
            {
                this.errorLogger.AppendLine($"Command {commandName} can only have 1 parameter at position {parameter.position}");
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