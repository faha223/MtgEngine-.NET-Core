using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Swamp", "", "", "")]
    public class SnowCoveredSwamp : BasicSnowLandSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicSnowLand(owner, ManaColor.Black, new[] { CardType.Land }, new[] { "Swamp" });
            card._attrs = CardAttrs;

            return card;
        }
    }
}
