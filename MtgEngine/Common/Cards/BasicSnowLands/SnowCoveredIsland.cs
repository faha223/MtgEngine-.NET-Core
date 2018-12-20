using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

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
