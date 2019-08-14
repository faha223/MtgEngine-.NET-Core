using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class ColorModifier : Modifier
    {
        public ManaColor Value { get; set; }

        public ColorModifier(IResolvable source, string property, ModifierMode mode, ManaColor value) : base(source, property, mode)
        {
            Value = value;
        }
    }
}
