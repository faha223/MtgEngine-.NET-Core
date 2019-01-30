using MtgEngine.Common.Cards;
using System;
using System.Linq;

namespace MtgEngine.Common.Costs
{
    public class SacrificeTargetCost : Cost
    {
        private Func<Card, bool> _targetSelector;

        public SacrificeTargetCost(IResolvable source, Func<Card, bool> targetSelector) : base(source)
        {
            _targetSelector = targetSelector;
        }

        public override bool CanPay()
        {
            var card = _source as Card;
            return card.Controller.Battlefield.Any(c => _targetSelector(c));
        }

        public override bool Pay()
        {
            var card = _source as Card;
            var target = card.Controller.SelectTarget("Choose a Target to Sacrifice", _targetSelector);
            if (target == null)
                return false;

            card.Controller.Sacrifice(target);
            return true;
        }
    }
}
