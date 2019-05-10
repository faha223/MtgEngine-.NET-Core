using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    /// <summary>
    /// Cards can, generally, be put on the stack. The exception to this rule is Land cards
    /// </summary>
    public partial class Card : ITarget, IResolvable
    {
        public bool UsesStack { get; }

        protected List<StaticAbility> _staticAbilities { get; } = new List<StaticAbility>();
        protected List<StaticAbility> _copiedCardStaticAbilities { get; set; }
        public List<StaticAbility> StaticAbilities
        {
            get
            {
                if (_copiedCardStaticAbilities != null)
                    return _copiedCardStaticAbilities;
                return _staticAbilities;
            }
        }

        public Guid InstanceId { get; } = Guid.NewGuid();

        protected Cost _cost { get; set; }
        protected Cost _copiedCardCost { get; set; }
        public virtual Cost Cost
        {
            get
            {
                if(_copiedCardCost != null)
                    return _copiedCardCost;
                return _cost;
            }
            protected set
            {
                _cost = value;
            }
        }

        protected CardType[] _types { get; }
        public virtual CardType[] Types { get { return _types; } }

        public virtual bool CanBeTargetedBy(IResolvable other)
        {            
            return true;
        }

        // This is virtual so that it can be overridden in token classes
        protected ManaColor[] _copiedCardColorIdentity { get; set; }
        public virtual ManaColor[] ColorIdentity {
            get
            {
                if (_copiedCardColorIdentity != null)
                    return _copiedCardColorIdentity;
                if (StaticAbilities.Contains(StaticAbility.Devoid))
                    return null;

                // TODO: Parse Mana Symbols from Text, and combine with mana symbols in ManaCost, then 
                // distill down to single mana colors (no hybrids), then distinct, sort, return as array;

                // Requires: ManaParser
                return null;
            }
        }

        protected string[] _subtypes { get; }
        protected string[] _copiedCardSubTypes { get; set; }
        public virtual string[] Subtypes
        {
            get
            {
                if (_copiedCardSubTypes != null)
                    return _copiedCardSubTypes;
                return _subtypes;
            }
        }

        protected bool _isBasic { get; }
        protected bool? _copiedCardIsBasic { get; set; }
        public virtual bool IsBasic
        {
            get
            {
                if (_copiedCardIsBasic.HasValue)
                    return _copiedCardIsBasic.Value;
                return _isBasic;
            }
        }

        protected bool _isLegendary { get; }
        protected bool? _copiedCardIsLegendary { get; set; }
        public virtual bool IsLegendary
        {
            get
            {
                if (_copiedCardIsLegendary.HasValue)
                    return _copiedCardIsLegendary.Value;
                return _isLegendary;
            }
        }

        protected bool _isSnow { get; }
        protected bool? _copiedCardIsSnow { get; set; }
        public virtual bool IsSnow
        {
            get
            {
                if (_copiedCardIsSnow.HasValue)
                    return _copiedCardIsSnow.Value;
                return _isSnow;
            }
        }

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
