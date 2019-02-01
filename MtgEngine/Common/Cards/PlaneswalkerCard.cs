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

        public override void AddCounters(int count, CounterType counter)
        {
            // Planeswalkers can get loyalty counters
            if (counter == CounterType.Loyalty)
            {
                for (int i = 0; i < count; i++)
                {
                    counters.Add(counter);
                }
            }
            else
            {
                // Otherwise treat this as a creature/permanent/thing
                base.AddCounters(count, counter);
            }
        }

        public override void TakeDamage(int amount, Card source)
        {
            RemoveCounters(amount, CounterType.Loyalty);
        }

        public override bool IsDead => !Counters.Contains(CounterType.Loyalty);
    }
}
