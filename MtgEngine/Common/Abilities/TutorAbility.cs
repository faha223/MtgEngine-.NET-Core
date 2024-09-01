using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using System;
using System.Collections.Generic;

namespace MtgEngine.Common.Abilities
{
    /// <summary>
    /// This ability tutors a card to the battlefield
    /// </summary>
    public abstract class TutorAbility : ActivatedAbility
    {
        protected Func<Card, bool> targetSelector { get; private set; }
        protected int count { get; private set; }
        protected List<Card> fetchedCards { get; private set; } = new List<Card>();

        public TutorAbility(Card source, Cost cost, Func<Card, bool> targetSelector, int count, string text) : base(source, cost, text)
        {
            this.targetSelector = targetSelector;
            this.count = count;
        }

        public abstract void OnFetchCompleted(Game game);

        public override void OnResolve(Game game)
        {
            // Search your library for the cards
            fetchedCards.AddRange(game.SearchLibraryForCards(Controller, 1, targetSelector));

            // Remove fetched cards from Library
            foreach(var card in fetchedCards)
            {
                Controller.Library.Remove(card);
            }

            // Call OnFetchCompleted to put the cards where they need to go
            OnFetchCompleted(game);
            fetchedCards.Clear();
        }
    }
}
