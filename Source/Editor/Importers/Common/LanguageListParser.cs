using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Importer.Editor
{
    public static class LanguageListParser
    {
        public static List<string> Parse(string assetPath, int contentsStartIndex)
        {
            var languages = new List<string>();
            string header;

            using (var stream = new StreamReader(assetPath))
            {
                header = stream.ReadLine() ?? string.Empty;
            }

            if (!string.IsNullOrEmpty(header))
            {
                var columns = header.Split(new[] { ',' });

                for (var i = contentsStartIndex; i < columns.Length; i++)
                {
                    var lang = columns[i].Trim();

                    if (languages.Contains(lang))
                    {
                        languages.Clear();
                        Debug.LogError($"Duplicated language id '{columns[i]}' at column {i}");
                        break;
                    }

                    if (!Regex.IsMatch(lang, "^[a-zA-Z]+$", RegexOptions.Compiled))
                    {
                        languages.Clear();
                        Debug.LogError($"Language id at column {i} must only contain characters in [a-zA-Z_]. Current value: '{columns[i]}'");
                        break;
                    }

                    languages.Add(lang);
                }
            }

            return languages;
        }
    }
}