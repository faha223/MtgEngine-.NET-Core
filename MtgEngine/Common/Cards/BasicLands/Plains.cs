using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Plains", "", "", "")]
    public class Plains : BasicLandCard
    {
        public Plains(Player owner) : base(owner, ManaColor.White, new[] { CardType.Land }, new[] { "Plains" }, false)
        {

        }
    }
}
