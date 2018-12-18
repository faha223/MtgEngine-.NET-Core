using MtgEngine.Common.Cards;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using System;
using System.Collections.Generic;
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

        public override bool OfferMulligan()
        {
            // PassPriorityPlayer Does not take mulligans
            return false;
        }

        public override Card SelectTarget(string message, Func<Card, bool> targetSelector)
        {
            return null;
        }

        public override void Discard()
        {
            Thread.Sleep(500);
            var rand = new Random();
            var index = Math.Abs(rand.Next()) % Hand.Count;
            Discard(Hand[index]);
        }

        public override void ScryChoice(List<Card> scryedCards, out IEnumerable<Card> cardsOnTop, out IEnumerable<Card> cardsOnBottom)
        {
            // No change
            cardsOnBottom = null;
            cardsOnTop = scryedCards;
        }

        public override List<AttackerDeclaration> DeclareAttackers()
        {
            // Never Attack
            return null;
        }

        public override List<BlockerDeclaration> DeclareBlockers(IEnumerable<CreatureCard> attackingCreatures)
        {
            return null;
        }
    }
}
