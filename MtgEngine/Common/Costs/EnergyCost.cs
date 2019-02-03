using MtgEngine.Common.Enums;
using System.Text;

namespace MtgEngine.Common.Costs
{
    public class EnergyCost : PayCountersCost
    {
        public EnergyCost(IResolvable source, int count) : base(source, CounterType.Energy, count)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
                sb.Append("{E}");
            return sb.ToString();
        }
    }
}
