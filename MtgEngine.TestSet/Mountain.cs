using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Mountain", "TestSet", "", "")]
    public class Mountain : Common.Cards.BasicLands.Mountain
    {
        public Mountain(Player owner) : base(owner)
        {
        }
    }
}
