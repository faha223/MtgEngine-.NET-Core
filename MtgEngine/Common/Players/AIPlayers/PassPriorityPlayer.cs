using MtgEngine.Common.Cards;
using MtgEngine.Common.Players.Actions;
using System;
using System.Threading;

namespace MtgEngine.Common.Players.AIPlayers
{
    public class PassPriorityPlayer : Player
    {
        public PassPriorityPlayer(string name, int startingLifeTotal, string deckList) : base(name, startingLifeTotal, deckList)
        {
        }

        public override ActionBase GivePriority(Player activePlayer, bool canPlaySorcerySpeedSpells)
        {
            Thread.Sleep(500);
            return new PassPriorityAction();
        }

        public override Card SelectTarget(string message, Func<Card, bool> targetSelector)
        {
            return null;
        }
    }
}
