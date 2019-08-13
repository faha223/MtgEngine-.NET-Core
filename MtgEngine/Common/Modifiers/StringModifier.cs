using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class StringModifier : Modifier
    {
        public string Value { get; set; }

        public StringModifier(IResolvable source, string property, string value) : base(source, property, ModifierMode.Override)
        {
            Value = value;
        }

        public StringModifier(IResolvable source, string property, ModifierMode mode, string value) : base(source, property, mode)
        {
            Value = value;
        }
    }
}
