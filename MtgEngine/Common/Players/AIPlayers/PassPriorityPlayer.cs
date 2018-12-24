﻿using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MtgEngine.Common.Players.AIPlayers
{
    public class PassPriorityPlayer : Player
    {
        // How long to wait, simulating a real player
        private readonly TimeSpan timeToThink = TimeSpan.FromMilliseconds(100);

        public PassPriorityPlayer(string name, int startingLifeTotal, string deckList) : base(name, startingLifeTotal, deckList)
        {
        }

        public override ActionBase GivePriority(Player activePlayer, bool canPlaySorcerySpeedSpells)
        {
            Thread.Sleep(timeToThink);
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
            Thread.Sleep(timeToThink);
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

        public override List<AttackerDeclaration> DeclareAttackers(List<Player> opponents)
        {
            // Never Attack
            return null;
        }

        public override IEnumerable<CreatureCard> SortBlockers(CreatureCard attacker, IEnumerable<CreatureCard> blockers)
        {
            // Never Block
            return blockers;
        }

        public override ManaColor? PayManaCost(string cost)
        {
            return null;
        }

        public override int GetValueForX(string cost)
        {
            return 0;
        }

        public override List<BlockerDeclaration> DeclareBlockers(List<CreatureCard> attackingCreatures)
        {
            return null;
        }
    }
}
