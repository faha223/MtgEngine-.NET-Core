using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Birds of Paradise", "LEA", "", "")]
    public class BirdsOfParadise : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Bird" }, false, 0, 1, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{G}");

            card.StaticAbilities.Add(StaticAbility.Flying);

            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.White), "{T}: Add {W}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Black), "{T}: Add {B}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {R}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Green), "{T}: Add {G}"));

            return card;
        }
    }
}
