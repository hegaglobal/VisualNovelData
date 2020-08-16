using System;
using System.Text;
using System.Collections.Generic;

namespace VisualNovelData.Parser
{
    public interface IArrayParser<T>
    {
        T[] Parse(string itemStr, StringBuilder errorLogger);

        T[] Parse(string itemStr, char[] separators, StringBuilder errorLogger);
    }

    public class ArrayParser<T, TParser> : IArrayParser<T> where TParser : IParser<T>, new()
    {
        private static readonly char[] _separators = new[] { ';' };

        private readonly TParser parser = new TParser();

        public T[] Parse(string itemStr, StringBuilder errorLogger)
            => Parse(itemStr, _separators, errorLogger);

        public T[] Parse(string itemStr, char[] separators, StringBuilder errorLogger)
        {
            var items = itemStr?.Trim()?.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<T>();

            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i].Trim();

                if (string.IsNullOrEmpty(item))
                    continue;

                if (!this.parser.TryParse(item, out var value))
                {
                    errorLogger.AppendLine($"Cannot parse {item} into {typeof(T).Name}");
                    continue;
                }

                list.Add(value);
            }

            return list.ToArray();
        }
    }
}