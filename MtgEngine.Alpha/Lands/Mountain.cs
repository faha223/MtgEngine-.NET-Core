using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Mountain", "LEA", "", "")]
    public class Mountain : Common.Cards.BasicLands.Mountain
    {
        public Mountain(Player owner) : base(owner)
        {
        }
    }
}
