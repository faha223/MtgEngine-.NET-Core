using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha
{ 
    [MtgCard("Forest", "LEA", "", "")]
    public class Forest : Common.Cards.BasicLands.Forest
    {
        public Forest(Player owner) : base(owner)
        {

        }
    }
}
