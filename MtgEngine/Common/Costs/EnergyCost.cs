using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Costs
{
    public class EnergyCost : PayCountersCost
    {
        public EnergyCost(IResolvable source, int count) : base(source, CounterType.Energy, count)
        {
        }
    }
}
