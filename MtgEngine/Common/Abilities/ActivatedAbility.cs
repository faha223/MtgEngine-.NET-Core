using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;

namespace MtgEngine.Common.Abilities
{
    public abstract class ActivatedAbility : Ability
    {
        public PermanentCard Source { get; private set; }

        public Cost Cost { get; private set; }

        protected ActivatedAbility(PermanentCard source, Cost cost, string text) : base(source.Controller, text)
        {
            Source = source;
            Cost = cost;
        }
    }
}
