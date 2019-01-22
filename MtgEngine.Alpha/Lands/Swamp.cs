using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{
    [MtgCard("Swamp", "LEA", "", "")]
    public class Swamp : Common.Cards.BasicLands.Swamp
    {
        public Swamp(Player owner) : base(owner)
        {
        }
    }
}
