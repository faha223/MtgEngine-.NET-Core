using MtgEngine.Common.Enums;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public abstract partial class PermanentCard : Card, IDamageable
    {
        protected int _basePower { get; }
        protected int? _copiedCardBasePower { get; set; }
        public virtual int BasePower
        {
            get
            {
                if (_copiedCardBasePower.HasValue)
                    return _copiedCardBasePower.Value;
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

        protected int _baseToughness { get; }
        protected int? _copiedCardBaseToughness { get; set; }
        public virtual int BaseToughness
        {
            get
            {
                if (_copiedCardBaseToughness.HasValue)
                    return _copiedCardBaseToughness.Value;
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
