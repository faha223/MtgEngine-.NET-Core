using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Fusion Elemental", "TestSet", "", "", null, "As the shards merged into the Maelstrom, their mana energies fused into new monstrosities.")]
    public class FusionElemental : CreatureCard
    {
        public FusionElemental(Player owner) : base(owner, null, new CardType[] { CardType.Creature }, new string[] { "Elemental" }, 8, 8, false, false)
        {
            Cost = ManaCost.Parse(this, "{W}{U}{B}{R}{G}");
        }
    }
}
