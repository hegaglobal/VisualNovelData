namespace VisualNovelData.Parser
{
    public readonly struct IntParser : IParser<int>
    {
        public bool TryParse(string str, out int result)
            => int.TryParse(str, out result);
    }
}