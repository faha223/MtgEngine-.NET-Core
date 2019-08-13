using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Plains", "", "", "")]
    public class SnowCoveredPlains : BasicSnowLandSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicSnowLand(owner, ManaColor.White, new[] { CardType.Land }, new[] { "Plains" });
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            return card;
        }
    }
}
