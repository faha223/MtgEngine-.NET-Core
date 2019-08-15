using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Tropical Island", "LEB", "", "", "({T}: Add {G} or {U})")]
    public class TropicalIsland : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, new[] { "Forest", "Island" }, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Green), "{T}: Add {G}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}"));

            return card;
        }
    }
}
