using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Counterspell", "TestSet", "", "", "Counter target spell")]
    public class Counterspell : Alpha.Instants.Counterspell
    {
        public Counterspell(Player owner) : base(owner) { }
    }
}
