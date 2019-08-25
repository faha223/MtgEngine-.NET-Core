using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Sorceries
{
    [MtgCard("Channel", "LEA", "", "", "Until end of turn, any time you could activate a mana ability, you may pay 1 life. If you do, add {C}.")]
    public class Channel : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Sorcery }, null, false);
            card._attrs = CardAttrs;
            card.Cost = ManaCost.Parse(card, "{G}{G}");

            // TODO: Until end of turn, any time you could activate a mana ability, you may pay 1 life. If you do, add {C}.

            return card;
        }
    }
}
