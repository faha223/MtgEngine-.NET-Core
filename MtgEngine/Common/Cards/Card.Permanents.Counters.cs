using MtgEngine.Common.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        public delegate void CountersEvent(Card card, IResolvable source, CounterType counterType, int amount);
        public event CountersEvent CountersCreated;
        public event CountersEvent CountersRemoved;

        // This isn't protected because we don't want inheriting classes modifying how counters are added or removed
        private List<CounterType> counters { get; } = new List<CounterType>();

        // This returns a copy so that this can't be used to modify the counters
        public ReadOnlyCollection<CounterType> Counters => new ReadOnlyCollection<CounterType>(counters);

        public void AddCounters(IResolvable source, int amount, CounterType counter)
        {
            for (int i = 0; i < amount; i++)
            {
                switch (counter)
                {
                    case CounterType.Plus1Plus1:
                        if (counters.Contains(CounterType.Minus1Minus1))
                            counters.Remove(CounterType.Minus1Minus1);
                        else
                            counters.Add(CounterType.Plus1Plus1);
                        break;
                    case CounterType.Minus1Minus1:
                        if (counters.Contains(CounterType.Plus1Plus1))
                            counters.Remove(CounterType.Plus1Plus1);
                        else
                            counters.Add(CounterType.Minus1Minus1);
                        break;
                    case CounterType.Charge:
                    case CounterType.Corpse:
                    case CounterType.Ice:
                    case CounterType.Spore:
                        counters.Add(counter);
                        break;
                    default:
                        // Other types can't be placed on this object
                        break;
                }
            }

            CountersCreated?.Invoke(this, source, counter, amount);
        }

        public void RemoveCounters(IResolvable source, int amount, CounterType counter)
        {
            for (int i = 0; i < amount; i++)
            {
                if (counters.Contains(counter))
                    counters.Remove(counter);
            }

            CountersRemoved?.Invoke(this, source, counter, amount);
        }

        public void ClearCounters()
        {
            counters.Clear();
        }
    }
}
