using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Swamp", "", "", "")]
    public class Swamp : BasicLandCardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicLandCard(owner, ManaColor.Black, new[] { CardType.Land }, new[] { "Swamp" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
