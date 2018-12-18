using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Island", "", "", "")]
    public class SnowCoveredIsland : BasicSnowLand
    {
        public SnowCoveredIsland(Player owner) : base(owner, ManaColor.Blue, new[] { CardType.Land }, new[] { "Island" })
        {
        }
    }
}
