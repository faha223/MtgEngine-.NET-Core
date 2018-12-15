using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Players.Actions
{
    public class PlayCardAction : ActionBase
    {
        public Card Card { get; private set; }

        public PlayCardAction(Card card) : base(ActionType.PlayCard)
        {
            Card = card;
        }
    }
}
