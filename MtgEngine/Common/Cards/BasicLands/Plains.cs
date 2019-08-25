using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Plains", "", "", "")]
    public class Plains : BasicLandCardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicLandCard(owner, ManaColor.White, new[] { CardType.Land }, new[] { "Plains" }, false);
            card._attrs = CardAttrs;

            return card;
        }
    }
}
