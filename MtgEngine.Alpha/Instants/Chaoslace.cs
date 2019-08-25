using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Chaoslace", "LEA", "", "", "Target spell or permanent becomes red. (Its mana symbols remain unchanged.)")]
    public class Chaoslace : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Instant }, null, false);
            card._attrs = CardAttrs;
            card.Cost = ManaCost.Parse(card, "{R}");

            // TODO: Target spell or permanent becomes red.

            return card;
        }
    }
}
