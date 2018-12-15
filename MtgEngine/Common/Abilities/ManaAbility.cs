using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Mana;

namespace MtgEngine.Common.Abilities
{
    public class ManaAbility : ActivatedAbility
    {
        public ManaAmount ManaGenerated { get; private set; }

        public ManaAbility(Card source, Cost cost, ManaAmount manaGenerated) : base(source, cost)
        {
            ManaGenerated = manaGenerated;
        }

        public override void OnResolve(Game game)
        {
            Controller.ManaPool.Add(ManaGenerated);
        }
    }
}
