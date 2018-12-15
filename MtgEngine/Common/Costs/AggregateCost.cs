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

        public override void Pay()
        {
            foreach (var cost in innerCosts)
                cost.Pay();
        }

        public AggregateCost(IResolvable source, params Cost[] costs) : base(source)
        {
            innerCosts = costs;
        }
    }
}
