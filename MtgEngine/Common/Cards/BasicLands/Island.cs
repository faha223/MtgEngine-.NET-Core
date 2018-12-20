using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Island", "", "", "")]
    public class Island : BasicLandCard
    {
        public Island(Player owner) : base(owner, ManaColor.Blue, new[] { CardType.Land }, new[] { "Island" }, false)
        {

        }
    }
}
