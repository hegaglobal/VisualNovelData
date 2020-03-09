using System;
using System.Collections.Generic;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;

namespace VisualNovelData.Parser
{
    public abstract class CsvParser
    {
        protected static Parser<TEntity> Create<TEntity, TMapping>(TMapping mapping)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>
        {
            var options = new Options('"', '\\', ',');
            var tokenizer = new RFC4180Tokenizer(options);
            var parserOptions = new CsvParserOptions(true, "//", tokenizer);
            var readerOptions = new CsvReaderOptions(new[] { "\r\n", "\n" });
            var parser = new CsvParser<TEntity>(parserOptions, mapping);

            return new Parser<TEntity>(parser, readerOptions);
        }

        protected static Parser<TEntity> Create<TEntity, TMapping>(TMapping mapping,
                                                                   CsvParserOptions parserOptions,
                                                                   CsvReaderOptions readerOptions)
            where TEntity : class, new()
            where TMapping : CsvMapping<TEntity>
        {
            if (parserOptions == null)
                throw new ArgumentNullException(nameof(parserOptions));

            if (readerOptions == null)
                throw new ArgumentNullException(nameof(readerOptions));

            var parser = new CsvParser<TEntity>(parserOptions, mapping);

            return new Parser<TEntity>(parser, readerOptions);
        }

        protected class Parser<T>
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
}