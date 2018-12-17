using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract class LandCard : Card
    {
        protected LandCard(Player owner, string name, string image, string cardId, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary) : 
            base(owner, name, image, cardId, false, null, types, subtypes, isBasic, isLegendary)
        {
            Cost = new NoCost(this);
        }
    }
}
