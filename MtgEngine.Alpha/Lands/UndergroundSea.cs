using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Underground Sea", "LEA", "", "", "({T}: Add {U} or {B})")]
    public class UndergroundSea : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, new[] { "Island", "Swamp" }, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}"));
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {B}"));

            return card;
        }
    }
}
