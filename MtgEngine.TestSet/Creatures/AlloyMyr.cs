using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Alloy Myr", "TestSet", "", "", "{T}: Add one mana of any color.", "With or without witnesses, the suns continued their prismatic dance.")]
    public class AlloyMyr : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact, CardType.Creature }, new[] { "Myr" }, false, 2, 2, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}");

            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.White), "{T}: Add {W}."));
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}."));
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Black), "{T}: Add {B}."));
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {R}."));
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Green), "{T}: Add {G}."));

            return card;
        }
    }
}
