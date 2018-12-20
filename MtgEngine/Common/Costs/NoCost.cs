﻿namespace MtgEngine.Common.Costs
{
    public class NoCost : Cost
    {
        public NoCost(IResolvable source) : base(source)
        {
        }

        public override bool CanPay()
        {
            return true;
        }

        public override void Pay()
        {
        }
    }
}