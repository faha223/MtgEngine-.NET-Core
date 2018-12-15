using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;

namespace MtgEngine.Common.Abilities
{
    public abstract class ActivatedAbility : Ability
    {
        public Cost Cost { get; private set; }

        protected ActivatedAbility(Card source, Cost cost) : base(source.Controller)
        {
            Cost = cost;
        }
    }
}
