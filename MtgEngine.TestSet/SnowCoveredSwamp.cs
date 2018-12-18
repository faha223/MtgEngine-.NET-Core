using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Snow Covered Swamp", "TestSet", "", "")]
    public class SnowCoveredSwamp : Common.Cards.BasicSnowLands.SnowCoveredSwamp
    {
        public SnowCoveredSwamp(Player owner) : base(owner)
        {
        }
    }
}
