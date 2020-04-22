namespace VisualNovelData.Data
{
    public static class ProgressTypeExtensions
    {
        public const string ALL = nameof(ALL);
        public const string START_TO_CURRENT = nameof(START_TO_CURRENT);
        public const string ONLY_CURRENT = nameof(ONLY_CURRENT);

        public static bool TryConvertProgressType(this string value, out QuestRow.QuestProgressType type)
        {
            if (string.IsNullOrEmpty(value))
            {
                type = QuestRow.QuestProgressType.All;
                return true;
            }

            var val = value.ToUpper();

            if (val.Equals(ALL))
            {
                type = QuestRow.QuestProgressType.All;
                return true;
            }

            if (val.Equals(START_TO_CURRENT))
            {
                type = QuestRow.QuestProgressType.StartToCurrent;
                return true;
            }

            if (val.Equals(ONLY_CURRENT))
            {
                type = QuestRow.QuestProgressType.OnlyCurrent;
                return true;
            }

            type = default;
            return false;
        }

        public static string ToKeywordString(this QuestRow.QuestProgressType type)
        {
            switch (type)
            {
                case QuestRow.QuestProgressType.All:
                    return ALL;

                case QuestRow.QuestProgressType.StartToCurrent:
                    return START_TO_CURRENT;

                case QuestRow.QuestProgressType.OnlyCurrent:
                    return ONLY_CURRENT;

                default:
                    return string.Empty;
            }
        }
    }
}