using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    /// <summary>
    /// Cards can, generally, be put on the stack. The exception to this rule is Land cards
    /// </summary>
    public abstract partial class Card : ITarget, IResolvable
    {
        public bool UsesStack { get; }

        public List<StaticAbility> StaticAbilities { get; } = new List<StaticAbility>();

        public Guid InstanceId { get; } = Guid.NewGuid();

        private Cost _cost { get; set; }
        public virtual Cost Cost { get { return _cost; } protected set { _cost = value; } }

        private CardType[] _types { get; }
        public virtual CardType[] Types { get { return _types; } }

        public virtual bool CanBeTargetedBy(IResolvable other)
        {            
            return true;
        }

        // This is virtual so that it can be overridden in token classes
        public virtual ManaColor[] ColorIdentity {
            get
            {
                if (StaticAbilities.Contains(StaticAbility.Devoid))
                    return null;

                // TODO: Parse Mana Symbols from Text, and combine with mana symbols in ManaCost, then 
                // distill down to single mana colors (no hybrids), then distinct, sort, return as array;

                // Requires: ManaParser
                return null;
            }
        }

        private string[] _subtypes { get; }
        public virtual string[] Subtypes { get { return _subtypes; } }

        private bool _isBasic { get; }
        public virtual bool IsBasic { get { return _isBasic; } }

        private bool _isLegendary { get; }
        public virtual bool IsLegendary { get { return _isLegendary; } }

        private bool _isSnow { get; }
        public virtual bool IsSnow { get { return _isSnow; } }

        public Player Controller { get; private set; }

        public Player Owner { get; }

        protected Card(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary, bool isSnow)
        {
            Owner = owner;
            Controller = owner;
            UsesStack = usesStack;
            _cost = Cost;
            _types = types;
            _subtypes = subtypes;
            _isBasic = isBasic;
            _isLegendary = isLegendary;
            _isSnow = isSnow;

            if (_cost == null)
                _cost = new NoCost(this);
        }

        public virtual bool CanCast(Game game)
        {
            return true;
        }

        public virtual void OnCast(Game game)
        {

        }

        /// <summary>
        /// The method that is called as the spell resolves. If a spell is exiled after it resolves, exile it in this method
        /// </summary>
        /// <param name="game"></param>
        public virtual void OnResolve(Game game)
        {
        }

        public void GiveControl(Player player)
        {
            Controller = player;
        }
    }
}
