using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public abstract class SpellCard : Card
    {
        protected SpellCard(Player owner, Cost cost, CardType[] types, string[] subtypes, bool isLegendary) :
            base(owner, true, cost, types, subtypes, false, isLegendary, false)
        {
        }
    }
}
