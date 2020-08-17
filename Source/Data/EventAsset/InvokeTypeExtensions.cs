namespace VisualNovelData.Data
{
    public static class InvokeTypeExtensions
    {
        public const string ALL = nameof(ALL);
        public const string START_TO_CURRENT = nameof(START_TO_CURRENT);
        public const string ONLY_CURRENT = nameof(ONLY_CURRENT);

        public static bool TryParse(this string value, out EventRow.EventInvokeType type)
        {
            if (string.IsNullOrEmpty(value))
            {
                type = EventRow.EventInvokeType.All;
                return true;
            }

            var val = value.ToUpper();

            if (val.Equals(ALL))
            {
                type = EventRow.EventInvokeType.All;
                return true;
            }

            if (val.Equals(START_TO_CURRENT))
            {
                type = EventRow.EventInvokeType.StartToCurrent;
                return true;
            }

            if (val.Equals(ONLY_CURRENT))
            {
                type = EventRow.EventInvokeType.OnlyCurrent;
                return true;
            }

            type = default;
            return false;
        }

        public static string ToKeyword(this EventRow.EventInvokeType type)
        {
            switch (type)
            {
                case EventRow.EventInvokeType.All:
                    return ALL;

                case EventRow.EventInvokeType.StartToCurrent:
                    return START_TO_CURRENT;

                case EventRow.EventInvokeType.OnlyCurrent:
                    return ONLY_CURRENT;

                default:
                    return string.Empty;
            }
        }
    }
}