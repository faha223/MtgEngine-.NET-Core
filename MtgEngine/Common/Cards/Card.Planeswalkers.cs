using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : ITarget
    {
        public int _startingLoyalty = 0;

        public Card(Player owner, Cost cost, CardType[] types, string[] subtypes, int startingLoyalty) : this(owner, true, cost, types, subtypes, false, true, false)
        {
            _startingLoyalty = startingLoyalty;
            AddCounters(this, startingLoyalty, CounterType.Loyalty);
        }
    }
}
