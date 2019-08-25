using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Island", "", "", "")]
    public class Island : BasicLandCardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicLandCard(owner, ManaColor.Blue, new[] { CardType.Land }, new[] { "Island" }, false);
            card._attrs = CardAttrs;

            return card;
        }
    }
}
