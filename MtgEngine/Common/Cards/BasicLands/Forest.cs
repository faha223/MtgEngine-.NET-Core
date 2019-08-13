using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Forest", "", "", "")]
    public class Forest : BasicLandCardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicLandCard(owner, ManaColor.Green, new[] { CardType.Land }, new[] { "Forest" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
