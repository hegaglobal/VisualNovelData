namespace VisualNovelData.Data
{
    public static class ProgressTypeExtensions
    {
        public const string StartToCurrent = "START_TO_CURRENT";
        public const string OnlyCurrent = "ONLY_CURRENT";

        public static bool TryConvertProgressType(this string value, out QuestRow.QuestProgressType type)
        {
            if (string.IsNullOrEmpty(value))
            {
                type = QuestRow.QuestProgressType.StartToCurrent;
                return true;
            }

            var val = value.ToUpper();

            if (val.Equals(StartToCurrent))
            {
                type = QuestRow.QuestProgressType.StartToCurrent;
                return true;
            }

            if (val.Equals(OnlyCurrent))
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
                case QuestRow.QuestProgressType.StartToCurrent:
                    return StartToCurrent;

                case QuestRow.QuestProgressType.OnlyCurrent:
                    return OnlyCurrent;

                default:
                    return string.Empty;
            }
        }
    }
}