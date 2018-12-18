using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Snow Covered Forest", "TestSet", "", "")]
    public class SnowCoveredForest : Common.Cards.BasicSnowLands.SnowCoveredForest
    {
        public SnowCoveredForest(Player owner) : base(owner)
        {
        }
    }
}
