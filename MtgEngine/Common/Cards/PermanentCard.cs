using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract class PermanentCard : Card
    {
        public PermanentCard(Player owner, string name, string image, string cardId, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary) : 
            base(owner, name, image, cardId, usesStack, cost, types, subtypes, false, isLegendary)
        {
        }
    }
}
