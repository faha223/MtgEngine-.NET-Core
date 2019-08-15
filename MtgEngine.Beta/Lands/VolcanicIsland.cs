using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Volcanic Island", "LEB", "", "", "({T}: Add {U} or {R})")]
    public class VolcanicIsland : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, new[] { "Island", "Mountain" }, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {R}"));

            return card;
        }
    }
}
