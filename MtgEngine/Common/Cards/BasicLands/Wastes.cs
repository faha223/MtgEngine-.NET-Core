using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Wastes", "", "", "")]
    public class Wastes : BasicLandCardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicLandCard(owner, ManaColor.Colorless, new[] { CardType.Land }, null, false);
            card._attrs = CardAttrs;

            return card;
        }
    }
}
