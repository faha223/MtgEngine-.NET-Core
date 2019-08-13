using MtgEngine.Common.Enums;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        private int _basePower { get; }
        public int BasePower
        {
            get
            {
                return _basePower;
            }
        }

        public int Power
        {
            get
            {
                return BasePower
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    + (2 * Counters.Count(c => c == CounterType.Plus2Plus0))
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        private int _baseToughness { get; }
        public int BaseToughness
        {
            get
            {
                return _baseToughness;
            }
        }

        public int Toughness
        {
            get
            {
                return BaseToughness
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    + (2 * Counters.Count(c => c == CounterType.Plus0Plus2))
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        public bool HasSummoningSickness { get; set; } = false;
    }
}
