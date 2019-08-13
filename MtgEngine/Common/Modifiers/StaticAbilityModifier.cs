using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class StaticAbilityModifier : Modifier
    {
        public StaticAbility? Value { get; set; }

        public StaticAbilityModifier(IResolvable source, string property, ModifierMode mode, StaticAbility? value) : base(source, property, mode)
        {
            Value = value;
        }
    }
}
