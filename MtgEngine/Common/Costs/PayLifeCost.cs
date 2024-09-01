using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Costs
{
    public class PayLifeCost : Cost
    {
        private int lifeCost;

        public PayLifeCost(IResolvable source, int amount) : base(source)
        {
            lifeCost = amount;
        }

        public override bool CanPay()
        {
            return _source.Controller.LifeTotal >= lifeCost;
        }

        public override Cost Copy(IResolvable newSource)
        {
            return new PayLifeCost(newSource, lifeCost);
        }

        public override bool Pay()
        {
            if (CanPay())
            {
                var card = _source is Card ? (_source as Card) : (_source as Ability).Source;
                _source.Controller.LoseLife(lifeCost, card);
                return true;
            }
            return false;
        }
    }
}
