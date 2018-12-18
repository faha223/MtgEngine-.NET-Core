using MtgEngine.Common.Cards;

namespace MtgEngine.TestSet
{
    [MtgCard("Swamp", "TestSet", "", "")]
    public class Swamp : Common.Cards.BasicLands.Swamp
    {
        public Swamp(Player owner) : base(owner)
        {
        }
    }
}
