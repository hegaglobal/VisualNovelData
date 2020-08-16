using System;
using System.Collections.Generic;

namespace VisualNovelData.CommandSystems
{
    public abstract class BaseCommand : ICommand
    {
        protected Converter converter { get; } = new Converter();

        public abstract void Invoke(in Segment<object> parameters);
    }
}