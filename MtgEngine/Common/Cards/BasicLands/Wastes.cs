using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    public class Wastes : BasicLandCard
    {
        [MtgCard("Wastes", "", "", "")]
        public Wastes(Player owner) : base(owner, ManaColor.Colorless, new[] { CardType.Land }, null, false)
        {
        }
    }
}
