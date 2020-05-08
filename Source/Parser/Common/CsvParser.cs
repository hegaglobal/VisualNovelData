using System;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;

namespace VisualNovelData.Parser
{
    public abstract class CsvParser
    {
        public static Parser<TEntity> Create<TEntity, TMapping>(TMapping mapping)
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

        public static Parser<TEntity> Create<TEntity, TMapping>(TMapping mapping,
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
    }
}