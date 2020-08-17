using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public partial class VseRow : CsvRow
    {
        public string Event { get; private set; }

        public string Stage { get; private set; }

        public int? MaxConstraint { get; private set; }

        public string Commands { get; private set; }

        public class Mapping : CsvMapping<VseRow>
        {
            public Mapping()
            {
                var col = 0;

                MapProperty(++col, x => x.Event, (x, v) => x.Event = v);
                MapProperty(++col, x => x.Stage, (x, v) => x.Stage = v);
                MapProperty(++col, x => x.MaxConstraint, (x, v) => x.MaxConstraint = v);
                MapProperty(++col, x => x.Commands, (x, v) => x.Commands = v);
            }
        }
    }
}