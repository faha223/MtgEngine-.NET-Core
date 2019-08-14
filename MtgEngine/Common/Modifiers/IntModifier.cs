using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class IntModifier : Modifier
    {
        public int Value { get; set; }

        public IntModifier(IResolvable source, string property, int value) : base(source, property, ModifierMode.Add)
        {
            Value = value;
        }
    }
}
