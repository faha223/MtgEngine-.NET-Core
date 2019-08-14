using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Taiga", "LEB", "", "", "({T}: Add {R} or {G})")]
    public class Taiga : Alpha.Lands.Taiga
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
