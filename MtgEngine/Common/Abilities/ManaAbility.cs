using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Mana;

namespace MtgEngine.Common.Abilities
{
    // Mana Abilities don't use the stack
    public class ManaAbility : ActivatedAbility
    {
        public ManaAmount ManaGenerated { get; private set; }

        public ManaAbility(PermanentCard source, Cost cost, ManaAmount manaGenerated, string text) : base(source, cost, text)
        {
            ManaGenerated = manaGenerated;
        }

        public override void OnResolve(Game game)
        {
            Source.Controller.ManaPool.Add(ManaGenerated);
        }

        public override Ability Copy(PermanentCard newSource)
        {
            return new ManaAbility(newSource, Cost, ManaGenerated, Text);
        }
    }
}
