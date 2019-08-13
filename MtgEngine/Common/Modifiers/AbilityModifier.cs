using MtgEngine.Common.Abilities;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class AbilityModifier : Modifier
    {
        public Ability Value { get; set; } 

        public AbilityModifier(IResolvable source, string property, ModifierMode mode, Ability value) : base(source, property, mode)
        {
            Value = value;
        }
    }
}
