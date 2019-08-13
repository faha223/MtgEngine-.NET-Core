using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Fusion Elemental", "TestSet", "", "", null, "As the shards merged into the Maelstrom, their mana energies fused into new monstrosities.")]
    public class FusionElemental : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new CardType[] { CardType.Creature }, new string[] { "Elemental" }, false, 8, 8, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
            card.Cost = ManaCost.Parse(card, "{W}{U}{B}{R}{G}");

            return card;        
        }
    }
}
