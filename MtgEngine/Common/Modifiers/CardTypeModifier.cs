using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class CardTypeModifier : Modifier
    {
        public CardType? Value { get; set; }

        public CardTypeModifier(IResolvable source, string property, ModifierMode mode, CardType? value) : base(source, property, mode)
        {
            Value = value;
        }
    }
}
