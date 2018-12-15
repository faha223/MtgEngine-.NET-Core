using MtgEngine.Common.Cards;
using System;
using System.Collections.Generic;
using System.Text;

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

        public override void Pay()
        {
            ((Card)_source).Tap();
        }
    }
}
