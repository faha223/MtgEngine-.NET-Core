using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    [MtgCard("Basalt Monolith", "LEA", "", "")]
    public class BasaltMonolith : ArtifactCard
    {
        public BasaltMonolith(Player owner) : base(owner, true, null, new[] { CardType.Artifact }, null, false, false)
        {
            Cost = ManaCost.Parse(this, "{3}");

            // {T}: Add {C}{C}{C}
            Abilities.Add(new ManaAbility(this, new TapCost(this), new ManaAmount(3, ManaColor.Colorless), "{T}: Add {C}{C}{C}"));

            // {3}: Untap Basalt Monolith
            Abilities.Add(new BasaltMonolithUntapAbility(this));
        }

        // Basalt Monolith doesn't untap during your untap step.
        public override bool UntapsDuringUntapStep => false;

        public class BasaltMonolithUntapAbility : ActivatedAbility
        {
            public BasaltMonolithUntapAbility(PermanentCard source) : base(source, null, "{3}: Untap Basalt Monolith")
            {
                Cost = ManaCost.Parse(this, "{3}");
            }

            public override Ability Copy(PermanentCard newSource)
            {
                return new BasaltMonolithUntapAbility(newSource);
            }

            public override void OnResolve(Game game)
            {
                Source.Untap();
            }
        }
    }
}
