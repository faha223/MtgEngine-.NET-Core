using MtgEngine.Common.Enums;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public abstract partial class PermanentCard : Card, IDamageable
    {
        protected int _basePower { get; }
        public virtual int BasePower
        {
            get { return _basePower; }
        }

        public int Power
        {
            get
            {
                return BasePower
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        protected int _baseToughness { get; }
        public virtual int BaseToughness
        {
            get { return _baseToughness; }
        }

        public int Toughness
        {
            get
            {
                return BaseToughness
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        public bool HasSummoningSickness { get; set; } = false;
    }
}
