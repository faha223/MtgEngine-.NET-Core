using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Tundra", "LEA", "", "", "({T}: Add {W} or {U})")]
    public class Tundra : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, new[] { "Plains", "Island" }, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
        
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {W}"));
            card.Abilities.Add(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {U}"));

            return card;
        }
    }
}
