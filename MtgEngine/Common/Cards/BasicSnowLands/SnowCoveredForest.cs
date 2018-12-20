using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Forest", "", "", "")]
    public class SnowCoveredForest : BasicSnowLand
    {
        public SnowCoveredForest(Player owner) : base(owner, ManaColor.Green, new[] { CardType.Land }, new[] { "Forest" })
        {
        }
    }
}
