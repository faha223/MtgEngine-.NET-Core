using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;
using System;
using System.Linq;

namespace MtgEngine.Common.Costs
{
    public class TapTargetCost : Cost
    {
        private string _targetSelectionMessage;
        private Func<Card, bool> _targetSelector;
        private readonly string _text;

        public TapTargetCost(IResolvable source, string targetSelectionMessage, Func<Card, bool> targetSelector, string text) : base(source)
        {
            _targetSelectionMessage = targetSelectionMessage;
            _targetSelector = targetSelector;
            _text = text;
        }

        public override bool CanPay()
        {
            // Get the controller of the source. The source has to have a controller, or else there's no way to select targets
            Player controller = (_source is Card) ? (_source as Card).Controller : (_source as ActivatedAbility)?.Source.Controller;

            if(controller != null)
            {
                return controller.Battlefield.Any(c => _targetSelector(c) && c is PermanentCard && !(c as PermanentCard).IsTapped);
            }

            return false;
        }

        public override bool Pay()
        {
            if(_source is Card)
            {
                var target = (_source as Card).Controller.SelectTarget(_targetSelectionMessage, _targetSelector);
                return true;
            }
            return false;
        }

        public override Cost Copy(IResolvable newSource)
        {
            return new TapTargetCost(newSource, _targetSelectionMessage, _targetSelector, _text);
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
