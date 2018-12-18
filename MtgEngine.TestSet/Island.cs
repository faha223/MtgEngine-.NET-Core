using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Island", "TestSet", "", "")]
    public class Island : Common.Cards.BasicLands.Island
    {
        public Island(Player owner) : base(owner)
        {
        }
    }
}
