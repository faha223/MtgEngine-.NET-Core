using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public class PlaneswalkerCard : PermanentCard
    {
        public PlaneswalkerCard(Player owner, Cost cost, CardType[] types, string[] subtypes, int startingLoyalty) : base(owner, true, cost, types, subtypes, false, true, false)
        {
            AddCounters(startingLoyalty, CounterType.Loyalty);
        }

        public override void TakeDamage(int amount, Card source)
        {
            RemoveCounters(amount, CounterType.Loyalty);
        }

        public override bool IsDead => !Counters.Contains(CounterType.Loyalty);
    }
}
