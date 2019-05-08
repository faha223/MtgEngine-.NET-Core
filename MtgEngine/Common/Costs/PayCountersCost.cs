using MtgEngine.Common.Counters;
using MtgEngine.Common.Enums;
using System.Linq;

namespace MtgEngine.Common.Costs
{
    public class PayCountersCost : Cost
    {
        protected CounterType type;
        protected int count;

        public PayCountersCost(IResolvable source, CounterType type, int count) : base(source)
        {
            this.type = type;
            this.count = count;
        }

        public override bool CanPay()
        {
            if (_controller == null)
                return false;
            if(_controller.Counters.Count(c => c == type) < count)
                return false;
            return true;
        }

        public override bool Pay()
        {
            if (CanPay())
            {
                _controller.RemoveCounters(count, type);
                return true;
            }
            return false;
        }

        public override Cost Copy(IResolvable newSource)
        {
            return new PayCountersCost(newSource, type, count);
        }

        public override string ToString()
        {
            return $"{count} {CounterAttribute.GetCounterAttribute(type).Name}";
        }
    }
}
