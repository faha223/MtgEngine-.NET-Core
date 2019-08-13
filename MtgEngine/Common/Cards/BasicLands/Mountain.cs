using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Mountain", "", "", "")]
    public class Mountain : BasicLandCardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicLandCard(owner, ManaColor.Red, new[] { CardType.Land }, new[] { "Mountain" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
