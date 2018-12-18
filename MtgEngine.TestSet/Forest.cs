using MtgEngine.Common.Cards;

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
