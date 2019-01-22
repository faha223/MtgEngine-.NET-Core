using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Island", "LEA", "", "")]
    public class Island : Common.Cards.BasicLands.Island
    {
        public Island(Player owner) : base(owner)
        {
        }
    }
}
