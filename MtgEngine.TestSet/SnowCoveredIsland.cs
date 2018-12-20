using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Snow Covered Island", "TestSet", "", "")]
    public class SnowCoveredIsland : Common.Cards.BasicSnowLands.SnowCoveredIsland
    {
        public SnowCoveredIsland(Player owner) : base(owner)
        {
        }
    }
}
