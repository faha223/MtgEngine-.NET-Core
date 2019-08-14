using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Bayou", "LEB", "", "", "({T}: Add {B} or {G})")]
    public class Bayou : Alpha.Lands.Bayou
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
