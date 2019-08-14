using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Beta.Lands
{
    [MtgCard("Scrubland", "LEB", "", "", "({T}: Add {W} or {B})")]
    public class Scrubland : Alpha.Lands.Scrubland
    {
        public override Card GetCard(Player owner)
        {
            var card = base.GetCard(owner);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
