using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Forest", "", "", "")]
    public class Forest : BasicLandCard
    {
        public Forest(Player owner) : base(owner, ManaColor.Green, new[] { CardType.Land }, new[] { "Forest" }, false)
        {

        }
    }
}
