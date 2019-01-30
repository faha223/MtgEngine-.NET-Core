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

            //{T}: Add {C}{C}{C}
            Abilities.Add(new ManaAbility(this, new TapCost(this), new ManaAmount(3, ManaColor.Colorless), "{T}: Add {C}{C}{C}"));

            // TODO: Add {3}: Untap Basalt Monolith

            // TODO: Basalt Monolith doesn't untap during your untap step.
        }
    }
}
