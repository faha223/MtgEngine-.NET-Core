using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Snow Covered Mountain", "TestSet", "", "")]
    public class SnowCoveredMountain : Common.Cards.BasicSnowLands.SnowCoveredMountain
    {
        public SnowCoveredMountain(Player owner) : base(owner)
        {
        }
    }
}
