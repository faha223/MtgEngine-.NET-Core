using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

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
