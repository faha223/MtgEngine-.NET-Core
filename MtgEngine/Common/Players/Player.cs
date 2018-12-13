using MtgEngine.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine
{
    public abstract class Player
    {
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

        public Player(string name, int startingLifeTotal)
        {
            Name = name;

            _startingLifeTotal = startingLifeTotal;
            LifeTotal = _startingLifeTotal;
        }
    }
}