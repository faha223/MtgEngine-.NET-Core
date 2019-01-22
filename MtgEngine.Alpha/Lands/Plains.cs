using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Plains", "LEA", "", "")]
    public class Plains : Common.Cards.BasicLands.Plains
    {
        public Plains(Player owner) : base(owner)
        {

        }
    }
}
