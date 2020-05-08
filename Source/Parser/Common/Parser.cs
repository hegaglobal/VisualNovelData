using System.Collections.Generic;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public sealed class Parser<T>
    {
        private readonly CsvParser<T> parser;
        private readonly CsvReaderOptions readerOptions;

        public Parser(CsvParser<T> parser, CsvReaderOptions readerOptions)
        {
            this.parser = parser;
            this.readerOptions = readerOptions;
        }

        public IEnumerable<CsvMappingResult<T>> Parse(string csvData)
            => this.parser.ReadFromString(this.readerOptions, csvData);
    }
}