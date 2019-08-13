using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public abstract class Modifier
    {
        public Modifier(IResolvable source, string property, ModifierMode mode)
        {
            Source = source;
            Property = property;
            Mode = mode;
        }

        public IResolvable Source { get; set; }

        public string Property { get; set; }

        public ModifierMode Mode { get;set; }
    }
}
