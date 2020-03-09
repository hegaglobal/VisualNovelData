namespace VisualNovelData.Parser
{
    public static class StringExtensions
    {
        public static bool StartsWith(this string str, char value)
        {
            return str[0] == value;
        }

        public static bool EndsWith(this string str, char value)
        {
            return str[str.Length - 1] == value;
        }
    }
}
