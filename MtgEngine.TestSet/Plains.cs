using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Plains", "TestSet", "", "")]
    public class Plains : Common.Cards.BasicLands.Plains
    {
        public Plains(Player owner) : base(owner)
        {

        }
    }
}
