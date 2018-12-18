using MtgEngine.Common.Cards;

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
