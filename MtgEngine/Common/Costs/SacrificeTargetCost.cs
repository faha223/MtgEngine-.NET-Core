using MtgEngine.Common.Cards;
using System;
using System.Linq;

namespace MtgEngine.Common.Costs
{
    public class SacrificeTargetCost : Cost
    {
        private Func<Card, bool> _targetSelector;
        private string text;

        public SacrificeTargetCost(IResolvable source, Func<Card, bool> targetSelector, string text) : base(source)
        {
            _targetSelector = targetSelector;
            this.text = text;
        }

        public override bool CanPay()
        {
            var card = _source as Card;
            return card.Controller.Battlefield.Any(c => _targetSelector(c));
        }

        public override bool Pay()
        {
            var card = _source as Card;
            var target = card.Controller.SelectTarget("Choose a Target to Sacrifice", _targetSelector) as PermanentCard;
            if (target == null)
                return false;

            card.Controller.Sacrifice(target);
            return true;
        }

        public override Cost Copy(IResolvable newSource)
        {
            return new SacrificeTargetCost(newSource, _targetSelector, text);
        }

        public override string ToString()
        {
            return text;
        }
    }
}
