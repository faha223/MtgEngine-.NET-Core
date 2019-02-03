using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;

namespace MtgEngine.Common.Abilities
{
    public abstract class ActivatedAbility : Ability
    {
        public Cost Cost { get; protected set; }

        protected ActivatedAbility(PermanentCard source, Cost cost, string text) : base(source, text)
        {
            Cost = cost;
        }
    }
}
