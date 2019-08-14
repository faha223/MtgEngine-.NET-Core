using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Palladium Myr", "TestSet", "", "", "{T}: Add {C}{C}.", "The myr are like the Glimmervoid: blank canvases on which to build grand creations.")]
    public class PalladiumMyr : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact, CardType.Creature }, new[] { "Myr" }, false, 2, 2, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}");

            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(2, ManaColor.Colorless), "{T}: Add {C}{C}."));

            return card;
        }
    }
}
