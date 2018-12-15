using MtgEngine.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtgEngine.Common.Costs
{
    public class TapTargetCost : Cost
    {
        private string _targetSelectionMessage;
        private Func<Card, bool> _targetSelector;

        public TapTargetCost(IResolvable source, string targetSelectionMessage, Func<Card, bool> targetSelector) : base(source)
        {
            _targetSelectionMessage = targetSelectionMessage;
            _targetSelector = targetSelector;
        }

        public override bool CanPay()
        {
            // Get the controller of the source. The source has to have a controller, or else there's no way to select targets
            Player controller = (_source is Card) ? (_source as Card).Controller : (_source as Ability)?.Controller;

            if(controller != null)
            {
                return controller.Battlefield.Any(c => _targetSelector(c) && !c.IsTapped);
            }

            return false;
        }

        public override void Pay()
        {
            if(_source is Card)
            {
                var target = (_source as Card).Controller.SelectTarget(_targetSelectionMessage, _targetSelector);
            }
        }
    }
}
