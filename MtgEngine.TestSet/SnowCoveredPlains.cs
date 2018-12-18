using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Snow Covered Plains", "TestSet", "", "")]
    public class SnowCoveredPlains : Common.Cards.BasicSnowLands.SnowCoveredPlains
    {
        public SnowCoveredPlains(Player owner) : base(owner)
        {
        }
    }
}
