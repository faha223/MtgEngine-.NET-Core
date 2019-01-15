using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Wastes", "TestSet", "", "")]
    public class Wastes : Common.Cards.BasicLands.Wastes
    {
        public Wastes(Player owner) : base(owner)
        {
        }
    }
}
