using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Island", "", "", "")]
    public class SnowCoveredIsland : BasicSnowLandSource
    {
        public override Card GetCard(Player owner)
        {
            var card = GetBasicSnowLand(owner, ManaColor.Blue, new[] { CardType.Land }, new[] { "Island" });
            card._attrs = CardAttrs;

            return card;
        }
    }
}
