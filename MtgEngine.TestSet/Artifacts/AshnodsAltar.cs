using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Artifacts
{
    [MtgCard("Ashnod's Altar", "TestSet", "", "", "Sacrifice a Creature: Add {C}{C}", "\"If you work at sawing up carcasses, you notice how the joints fit, how the nerves are arrayed, and how the skin peels back.\" —Ashnod, to Tawnos")]
    public class AshnodsAltar : Card
    {
        public AshnodsAltar(Player owner) : base(owner, new[] { CardType.Artifact }, null, false, false)
        {
            Cost = ManaCost.Parse(this, "{3}");
            Abilities.Add(new ManaAbility(this, new SacrificeCreatureCost(this), new Common.Mana.ManaAmount(2, ManaColor.Colorless), "Sacrifice a Creature: Add {C}{C}"));
        }
    }
}
