using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Island", "TestSet", "", "")]
    public class Island : Common.Cards.BasicLands.Island
    {
        public Island(Player owner) : base(owner)
        {
        }
    }
}
