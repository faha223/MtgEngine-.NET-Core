using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Forest", "", "", "")]
    public class SnowCoveredForest : BasicSnowLandSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicSnowLand(owner, ManaColor.Green, new[] { CardType.Land }, new[] { "Forest" });
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
