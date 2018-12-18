using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine
{
    public abstract class Player
    {
        public int MulligansTaken { get; set; } = 0;

        public string Name { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public Zone Hand { get; } = new Zone();

        public List<Card> Library { get; } = new List<Card>();

        public Zone Battlefield { get; } = new Zone();

        public Zone Graveyard { get; } = new Zone();

        public Zone Exile { get; } = new Zone();

        private int _startingLifeTotal { get; } = 20;
        public int LifeTotal { get; set; } = 20;

        public int LandsPlayedThisTurn = 0;
        public int MaxLandsPlayedThisTurn = 1;

        protected int _maxHandSize = 7;
        public int MaxHandSize { get { return _maxHandSize; } }

        public ManaPool ManaPool { get; } = new ManaPool();

        public Dictionary<string, int> Counters { get; }
        public Zone CommandZone { get; } = new Zone();

        public int RollInitiative()
        {
            Random rand = new Random();
            return 1 + (rand.Next() % 100);
        }

        /// <summary>
        /// This method sends a request to the Player to request a target.
        /// </summary>
        /// <param name="targetSelector"></param>
        /// <returns></returns>
        public abstract Card SelectTarget(string message, Func<Card, bool> targetSelector);

        public void ShuffleLibrary()
        {
            Common.Utilities.LibraryShuffler.ShuffleLibrary(Library);
        }

        public void DrawHand(int handSize)
        {
            var cardsDrawn = Library.Take(handSize);
            Library.RemoveRange(0, handSize);
            Hand.AddRange(cardsDrawn);
        }

        public abstract bool OfferMulligan();

        public virtual void Draw(int howMany)
        {
            var cardsDrawn = Library.Take(howMany);
            Library.RemoveRange(0, howMany);
            Hand.AddRange(cardsDrawn);
        }

        public Player(string name, int startingLifeTotal, string deckList)
        {
            Name = name;

            _startingLifeTotal = startingLifeTotal;
            LifeTotal = _startingLifeTotal;
            Library = new Deck(this, deckList);
        }

        public abstract ActionBase GivePriority(Player activePlayer, bool canPlaySorcerySpeedSpells);

        public abstract List<AttackerDeclaration> DeclareAttackers();

        public abstract List<BlockerDeclaration> DeclareBlockers(IEnumerable<CreatureCard> AttackingCreatures);

        public virtual void DiscardToHandSize()
        {
            while (Hand.Count > MaxHandSize)
            {
                Discard();
            }
        }

        public abstract void Discard();

        public virtual void Discard(Card card)
        {
            if (!Hand.Contains(card))
                return;

            Hand.Remove(card);
            Graveyard.Add(card);
        }

        /// <summary>
        /// Show the player the top N cards of their library. They choose which ones that would like to put back on top of their library, and which ones they'd like to go on the bottom, and the order of each.
        /// </summary>
        /// <param name="scryedCards">The top N cards of their library</param>
        /// <param name="cardsOnTop">The cards the player wishes to put on the top of their library, in the order in which they wish to draw them</param>
        /// <param name="cardsOnBottom">The cards the player wishes to put on the bottom of their library, in the order they wish to draw them</param>
        public abstract void ScryChoice(List<Card> scryedCards, out IEnumerable<Card> cardsOnTop, out IEnumerable<Card> cardsOnBottom);
    }
}