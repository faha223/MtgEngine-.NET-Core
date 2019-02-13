using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MtgEngine.Common.Players
{
    public abstract class Player : ITarget, IDamageable
    {
        public int MulligansTaken { get; set; } = 0;

        public string Name { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public Zone Hand { get; } = new Zone();

        public List<Card> Library { get; } = new List<Card>();

        private List<CounterType> counters { get; } = new List<CounterType>();
        public ReadOnlyCollection<CounterType> Counters => new ReadOnlyCollection<CounterType>(counters);

        public delegate void CountersEvent(Player player, CounterType counterType, int count);
        public event CountersEvent CountersCreated;
        public event CountersEvent CountersRemoved;        

        public void AddCounters(int amount, CounterType counter)
        {
            for (int i = 0; i < amount; i++)
            {
                switch (counter)
                {
                    case CounterType.Energy:
                    case CounterType.Poison:
                        counters.Add(counter);
                        break;
                    default:
                        // Other types can't be placed on this object
                        break;
                }
            }
            CountersCreated?.Invoke(this, counter, amount);
        }

        public void RemoveCounters(int amount, CounterType counter)
        {
            for (int i = 0; i < amount; i++)
            {
                if (counters.Contains(counter))
                    counters.Remove(counter);
            }
            CountersRemoved?.Invoke(this, counter, amount);
        }

        public Zone Battlefield { get; } = new Zone();

        public Zone Graveyard { get; } = new Zone();

        public Zone Exile { get; } = new Zone();

        private int _startingLifeTotal { get; } = 20;
        public int LifeTotal { get; set; } = 20;

        public void LoseLife(int amount)
        {
            LifeTotal -= amount;
        }

        public void GainLife(int amount)
        {
            LifeTotal += amount;
        }

        public void TakeDamage(int amount, Card source)
        {
            if((source is PermanentCard) && (source as PermanentCard).HasInfect)
                AddCounters(amount, CounterType.Poison);
            else
                LoseLife(amount);
        }

        public bool IsDead => LifeTotal <= 0;

        public int LandsPlayedThisTurn = 0;
        public int MaxLandsPlayedThisTurn = 1;

        protected int _maxHandSize = 7;
        public int MaxHandSize { get { return _maxHandSize; } }

        public ManaPool ManaPool { get; } = new ManaPool();

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

        public abstract ManaColor? PayManaCost(string cost);

        public abstract List<Card> MakeChoice(string message, int count, List<Card> options);

        public abstract List<PermanentCard> MakeChoice(string message, int count, List<PermanentCard> options);

        public abstract List<PermanentCard> Sort(string message, List<PermanentCard> options);

        public abstract List<Card> Sort(string message, List<Card> options);

        public abstract int GetValueForX(string cost);

        public void DrawHand(int handSize)
        {
            var cardsDrawn = Library.Take(handSize);
            Library.RemoveRange(0, handSize);
            Hand.AddRange(cardsDrawn);
        }

        public abstract bool OfferMulligan();

        public virtual void Draw(int howMany)
        {
            // TODO: Throw PlayerLostGame exception if the player is forced to draw more cards than are in their library.
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

        public abstract ActionBase GivePriority(Game game, bool canPlaySorcerySpeedSpells);

        public abstract List<AttackerDeclaration> DeclareAttackers(List<Player> opponents);

        public abstract List<BlockerDeclaration> DeclareBlockers(List<PermanentCard> AttackingCreatures);

        public abstract ITarget ChooseTarget(IResolvable source, List<ITarget> possibleTargets);

        public abstract ManaColor ChooseColor();

        public virtual void DiscardToHandSize()
        {
            while (Hand.Count > MaxHandSize)
            {
                Discard();
            }
        }

        public abstract void Discard();

        public abstract IEnumerable<PermanentCard> SortBlockers(PermanentCard attacker, IEnumerable<PermanentCard> blockers);

        public virtual void Discard(Card card)
        {
            if (!Hand.Contains(card))
                return;

            Hand.Remove(card);
            Graveyard.Add(card);
        }

        public virtual bool Sacrifice(Card card)
        {
            if (!Battlefield.Contains(card))
                return false;

            Battlefield.Remove(card);
            card.Owner.Graveyard.Add(card);
            return true;
        }

        #region Game Event Handlers

        public virtual void CardHasChangedZones(Game game, Card card, Enums.Zone oldZone, Enums.Zone newZone)
        {
        }

        public virtual void AbilityHasEnteredStack(Game game, Ability ability)
        {
        }

        public virtual void GameStepChanged(Game game, string currentStep)
        {
        }

        public virtual void PlayerTookDamage(Game game, Player player, int damageDealt)
        {
        }

        public virtual void PlayerLostTheGame(Game game, Player player, string reason)
        {
        }

        public virtual void GameEndedInDraw(Game game)
        {
        }

        public virtual void PlayerWonTheGame(Game game, Player player, string reason)
        {
        }

        #endregion Game Event Handlers

        /// <summary>
        /// Show the player the top N cards of their library. They choose which ones that would like to put back on top of their library, and which ones they'd like to go on the bottom, and the order of each.
        /// </summary>
        /// <param name="scryedCards">The top N cards of their library</param>
        /// <param name="cardsOnTop">The cards the player wishes to put on the top of their library, in the order in which they wish to draw them</param>
        /// <param name="cardsOnBottom">The cards the player wishes to put on the bottom of their library, in the order they wish to draw them</param>
        public abstract void ScryChoice(List<Card> scryedCards, out IEnumerable<Card> cardsOnTop, out IEnumerable<Card> cardsOnBottom);

        #region Helper Methods

        protected bool canPlayCardThisTurn(Card card, Game game, bool canPlaySorcerySpeedSpells)
        {
            bool canPlayCard = card.CanCast(game);
            canPlayCard &= (canPlaySorcerySpeedSpells || card.IsAnInstant || card.StaticAbilities.Contains(StaticAbility.Flash));
            canPlayCard &= (!card.IsALand || LandsPlayedThisTurn < MaxLandsPlayedThisTurn);

            return canPlayCard;
        }

        #endregion Helper Methods
    }
}