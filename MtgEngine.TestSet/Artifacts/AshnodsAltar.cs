using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Artifacts
{
    [MtgCard("Ashnod's Altar", "TestSet", "", "", "Sacrifice a Creature: Add {C}{C}", "\"If you work at sawing up carcasses, you notice how the joints fit, how the nerves are arrayed, and how the skin peels back.\" —Ashnod, to Tawnos")]
    public class AshnodsAltar : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact }, null, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}");
            card.AddAbility(new ManaAbility(card, new SacrificeCreatureCost(card), new Common.Mana.ManaAmount(2, ManaColor.Colorless), "Sacrifice a Creature: Add {C}{C}."));

            return card;
        }
    }
}
