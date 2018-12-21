﻿using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Costs
{
    public class TapCost : Cost
    {
        public TapCost(IResolvable source) : base(source)
        {

        }

        public override bool CanPay()
        {
            return !((Card)_source).IsTapped;
        }

        public override bool Pay()
        {
            ((Card)_source).Tap();
            return true;
        }

        public override string ToString()
        {
            return "{T}";
        }
    }
}
