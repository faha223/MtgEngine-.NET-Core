using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Badlands", "LEB", "", "", "({T}: Add {B} or {R})")]
    public class Badlands : Alpha.Lands.Badlands
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
