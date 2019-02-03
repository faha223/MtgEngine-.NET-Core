using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Costs
{
    public class TapCost : Cost
    {
        public TapCost(PermanentCard source) : base(source)
        {

        }

        public override bool CanPay()
        {
            return !(_source as PermanentCard).IsTapped;
        }

        public override bool Pay()
        {
            (_source as PermanentCard).Tap();
            return true;
        }

        public override string ToString()
        {
            return "{T}";
        }
    }
}
