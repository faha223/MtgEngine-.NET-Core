using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{ 
    [MtgCard("Forest", "TestSet", "", "")]
    public class Forest : Common.Cards.BasicLands.Forest
    {
        public Forest(Player owner) : base(owner)
        {

        }
    }
}
