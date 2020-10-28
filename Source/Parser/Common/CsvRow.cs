using System.Text;
using System.Text.RegularExpressions;

namespace VisualNovelData.Parser
{
    public abstract class CsvRow
    {
        protected const string IdCharRange = "[a-zA-Z0-9_-]";

        protected readonly StringBuilder error = new StringBuilder();
        protected readonly Regex idRegex = new Regex("^[a-zA-Z0-9_-]+$");
        protected readonly StringBuilder stringBuilder = new StringBuilder();

        public bool IsError
            => this.error.Length > 0;

        public string Error
            => this.error.ToString();

        public abstract bool IsEmpty { get; }
    }
}