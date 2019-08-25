using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    [MtgCard("Celestial Prism", "LEA", "", "", "{2}{T}: Add one mana of any color.")]
    public class CelestialPrism : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact }, null, false);
            card._attrs = CardAttrs;

            card.Cost = ManaCost.Parse(card, "{3}");

            card.AddAbility(new ManaAbility(card, new AggregateCost(card, ManaCost.Parse(card, "{2}"), new TapCost(card)), new ManaAmount(1, ManaColor.White), "{2}{T}: Add {W}."));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, ManaCost.Parse(card, "{2}"), new TapCost(card)), new ManaAmount(1, ManaColor.Blue), "{2}{T}: Add {U}."));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, ManaCost.Parse(card, "{2}"), new TapCost(card)), new ManaAmount(1, ManaColor.Black), "{2}{T}: Add {B}."));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, ManaCost.Parse(card, "{2}"), new TapCost(card)), new ManaAmount(1, ManaColor.Red), "{2}{T}: Add {R}."));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, ManaCost.Parse(card, "{2}"), new TapCost(card)), new ManaAmount(1, ManaColor.Green), "{2}{T}: Add {G}."));

            return card;
        }
    }
}
