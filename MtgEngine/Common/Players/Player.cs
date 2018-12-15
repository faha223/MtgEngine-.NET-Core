using MtgEngine.Common.Cards;
using MtgEngine.Common.Players.Actions;
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

        public List<Card> Hand { get; } = new List<Card>();

        public List<Card> Library { get; } = new List<Card>();

        public List<Card> Battlefield { get; } = new List<Card>();

        public List<Card> Graveyard { get; } = new List<Card>();

        public List<Card> Exile { get; } = new List<Card>();

        private int _startingLifeTotal { get; } = 20;
        public int LifeTotal { get; set; } = 20;

        public Dictionary<string, int> Counters { get; }
        public List<Card> CommandZone { get; } = new List<Card>();

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

        public void Draw(int howMany)
        {
            IEnumerable<Card> cardsDrawn = Library.Take(howMany);
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
    }
}