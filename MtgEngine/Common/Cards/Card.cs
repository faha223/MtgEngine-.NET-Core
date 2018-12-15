using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using System;

namespace MtgEngine.Common.Cards
{
    /// <summary>
    /// Cards can, generally, be put on the stack. The exception to this rule is Land cards
    /// </summary>
    public abstract class Card : IResolvable
    {
        private string _name { get; }
        public virtual string Name { get { return _name; } }

        public bool UsesStack { get; }

        public string Image { get; }

        public string CardId { get; }

        public Guid InstanceId { get; } = Guid.NewGuid();

        private Cost _cost { get; }
        public virtual Cost Cost { get { return _cost; } }

        private CardType[] _types { get; }
        public virtual CardType[] Types { get { return _types; } }

        private string[] _subtypes { get; }
        public virtual string[] Subtypes { get { return _subtypes; } }

        private bool _isBasic { get; }
        public virtual bool IsBasic { get { return _isBasic; } }

        private bool _isLegendary { get; }
        public virtual bool IsLegendary { get { return _isLegendary; } }

        public bool IsTapped { get; protected set; }

        public Player Controller { get; private set; }

        public Player Owner { get; }

        protected Card(Player owner, string name, string image, string cardId, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary)
        {
            Owner = owner;
            Controller = owner;
            Image = image;
            CardId = cardId;
            UsesStack = usesStack;
            _name = name;
            _cost = Cost;
            _types = types;
            _subtypes = subtypes;
            _isBasic = isBasic;
            _isLegendary = isLegendary;
        }

        public virtual void OnCast(Game game)
        {

        }

        public virtual void OnResolve(Game game)
        {
        }

        /// <summary>
        /// Permanents are added to the Battlefield, Spells go to their owner's graveyard unless specified otherwise
        /// </summary>
        /// <param name="game"></param>
        public abstract void AfterResolve(Game game);

        public void Untap()
        {
            if (IsTapped)
                IsTapped = false;
        }

        public void Tap()
        {
            if (!IsTapped)
                IsTapped = true;
        }

        public void GiveControl(Player player)
        {
            Controller = player;
        }
    }
}
