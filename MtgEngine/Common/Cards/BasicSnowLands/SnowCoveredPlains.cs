using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Plains", "", "", "")]
    public class SnowCoveredPlains : BasicSnowLand
    {
        public SnowCoveredPlains(Player owner) : base(owner, ManaColor.White, new[] { CardType.Land }, new[] { "Plains" })
        {
        }
    }
}
