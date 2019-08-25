using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Mountain", "", "", "")]
    public class SnowCoveredMountain : BasicSnowLandSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicSnowLand(owner, ManaColor.Red, new[] { CardType.Land }, new[] { "Mountain" });
            card._attrs = CardAttrs;

            return card;
        }
    }
}
