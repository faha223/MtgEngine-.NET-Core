﻿using System.Linq;

namespace MtgEngine.Common.Costs
{
    /// <summary>
    /// This class is used when multiple types of costs are required.
    /// ex. Altar's Reap requires that you sacrifice a creature in addition to paying its mana cost
    /// </summary>
    public class AggregateCost : Cost
    {
        private Cost[] innerCosts;

        public override bool CanPay()
        {
            foreach (var cost in innerCosts)
                if (!cost.CanPay())
                    return false;
            return true;
        }

        public override bool Pay()
        {
            foreach (var cost in innerCosts)
                if (!cost.Pay())
                    return false;
            return true;
        }

        public AggregateCost(IResolvable source, params Cost[] costs) : base(source)
        {
            innerCosts = costs;
        }
        
        public override Cost Copy(IResolvable newSource)
        {
            return new AggregateCost(newSource, innerCosts.Select(c => c.Copy(newSource)).ToArray());
        }

        public override string ToString()
        {
            return string.Join(", ", innerCosts.Select(c => c.ToString()));
        }
    }
}
