using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public partial class Card : ITarget
    {
        public Card(Player owner, Cost cost, CardType[] types, string[] subtypes, int startingLoyalty) : this(owner, true, cost, types, subtypes, false, true, false)
        {
            AddCounters(this, startingLoyalty, CounterType.Loyalty);
        }
    }
}
