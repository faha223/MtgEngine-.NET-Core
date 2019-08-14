using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class EffectModifier : Modifier
    {
        public Effect Value { get; set; }

        public EffectModifier(IResolvable source, string property, ModifierMode mode, Effect value) : base(source, property, mode)
        {
            Value = value;
        }
    }
}
