using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Savannah", "LEB", "", "", "({T}: Add {G} or {W})")]
    public class Savannah : Alpha.Lands.Savannah
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
