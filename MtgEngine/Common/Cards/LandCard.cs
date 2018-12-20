using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public abstract class LandCard : PermanentCard
    {
        protected LandCard(Player owner, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary, bool isSnow) : 
            base(owner, false, null, types, subtypes, isBasic, isLegendary, isSnow)
        {
            Cost = new NoCost(this);
        }
    }
}
