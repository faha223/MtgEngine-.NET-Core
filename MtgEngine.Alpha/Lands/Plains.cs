using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Plains", "LEA", "", "")]
    public class Plains : Common.Cards.BasicLands.Plains
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = CardAttrs;

            return card;
        }
    }
}
