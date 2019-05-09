using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using MtgEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override ActionBase GivePriority(Game game, bool canPlaySorcerySpeedSpells)
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

        public override IEnumerable<PermanentCard> SortBlockers(PermanentCard attacker, IEnumerable<PermanentCard> blockers)
        {
            // Never Block
            return blockers;
        }

        public override ITarget ChooseTarget(IResolvable source, List<ITarget> possibleTargets)
        {
            return possibleTargets.Last();
        }

        public override Player ChoosePlayer(string message, IEnumerable<Player> playerOptions)
        {
            return playerOptions.ToList().Random();
        }

        public override ManaColor? PayManaCost(string cost)
        {
            return null;
        }

        public override List<PermanentCard> MakeChoice(string message, int count, List<PermanentCard> options)
        {
            return options.Take(count).ToList();
        }

        public override List<Card> MakeChoice(string message, int count, List<Card> options)
        {
            return options.Take(count).ToList();
        }

        public override List<PermanentCard> Sort(string message, List<PermanentCard> options)
        {
            return options;
        }

        public override List<Card> Sort(string message, List<Card> options)
        {
            return options;
        }

        public override int GetValueForX(string cost)
        {
            return 0;
        }

        public override ManaColor ChooseColor()
        {
            var rand = new Random();
            var options = new[] { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green };

            return options[Math.Abs(rand.Next()) % options.Length];
        }

        public override List<BlockerDeclaration> DeclareBlockers(List<PermanentCard> attackingCreatures)
        {
            return null;
        }
    }
}
