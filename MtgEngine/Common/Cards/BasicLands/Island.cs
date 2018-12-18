using MtgEngine.Common.Enums;

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
