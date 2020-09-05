using System;
using System.Collections.Generic;

namespace VisualNovelData.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected Converter converter { get; } = new Converter();

        public abstract void Invoke(in Metadata metadata, in Segment<object> parameters);
    }
}