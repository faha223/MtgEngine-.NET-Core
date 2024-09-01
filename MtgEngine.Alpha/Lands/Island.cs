using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Island", "LEA", "", "")]
    public class Island : Common.Cards.BasicLands.Island
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = CardAttrs;

            return card;
        }
    }
}
