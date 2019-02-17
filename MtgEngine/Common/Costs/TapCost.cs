using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Costs
{
    public class TapCost : Cost
    {
        private PermanentCard _src;
        public TapCost(PermanentCard source) : base(source)
        {
            _src = source;
        }

        public override bool CanPay()
        {
            // Permanents can't pay tap cost if they're already tapped
            if (_src.IsTapped)
                return false;

            // Creatures can't pay tap cost if they have summoning sickness
            if (_src.IsACreature && _src.HasSummoningSickness)
                return false;

            // If we passed all the tests, then we can pay the price
            return true;
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
