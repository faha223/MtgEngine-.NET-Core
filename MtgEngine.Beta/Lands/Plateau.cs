using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Plateau", "LEB", "", "", "({T}: Add {U} or {R})")]
    public class Plateau : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, new[] { "Mountain", "Plains" }, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {R}"));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.White), "{T}: Add {W}"));

            return card;
        }
    }
}
