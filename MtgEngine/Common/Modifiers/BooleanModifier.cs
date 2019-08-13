using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class BooleanModifier : Modifier
    {
        public bool Value { get; set; }
        public BooleanModifier(IResolvable source, string property, bool value) : base(source, property, ModifierMode.Override)
        {
            Value = value;
        }
    }
}
