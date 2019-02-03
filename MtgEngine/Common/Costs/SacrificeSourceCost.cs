using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Costs
{
    public class SacrificeSourceCost : Cost
    {
        public SacrificeSourceCost(IResolvable source) : base(source)
        {
        }

        public override bool CanPay()
        {
            var card = _source as Card;
            return card.Controller.Battlefield.Contains(card);
        }

        public override bool Pay()
        {
            if (CanPay())
            {
                var card = _source as Card;
                card.Controller.Sacrifice(card);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Sacrifice {(_source as Card).Name}";
        }
    }
}
