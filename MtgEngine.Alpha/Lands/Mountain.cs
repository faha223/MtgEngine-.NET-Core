using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Mountain", "LEA", "", "")]
    public class Mountain : Common.Cards.BasicLands.Mountain
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = CardAttrs;

            return card;
        }
    }
}
