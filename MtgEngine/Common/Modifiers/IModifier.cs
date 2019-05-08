using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public interface IModifier
    {
        Card Source { get; set; }

        string Property { get; set; }

        ModifierMode Mode { get;set; }
    }
}
