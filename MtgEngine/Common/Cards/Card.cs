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
    public sealed partial class Card : ITarget, IResolvable
    {
        public bool UsesStack { get; }

        private Dictionary<string, object> vars = new Dictionary<string, object>();
        public void SetVar(string variable, object value)
        {
            if (vars.ContainsKey(variable))
                vars.Remove(variable);
            vars.Add(variable, value);
        }

        public T GetVar<T>(string variable)
        {
            if (vars.ContainsKey(variable) && vars[variable] is T)
                return (T)vars[variable];

            return default(T);
        }

        private List<StaticAbility> _staticAbilities { get; } = new List<StaticAbility>();
        public List<StaticAbility> StaticAbilities
        {
            get
            {
                return _staticAbilities;
            }
        }

        public Guid InstanceId { get; } = Guid.NewGuid();

        private Cost _cost { get; set; }
        public Cost Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }

        private CardType[] _types { get; }
        public CardType[] Types { get { return _types; } }

        // This is virtual so that it can be overridden in token classes
        public ManaColor[] ColorIdentity
        {
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
        public string[] Subtypes
        {
            get
            {
                return _subtypes;
            }
        }

        private bool _isBasic { get; }
        public bool IsBasic
        {
            get
            {
                return _isBasic;
            }
        }

        private bool _isLegendary { get; }
        public bool IsLegendary
        {
            get
            {
                return _isLegendary;
            }
        }

        private bool _isSnow { get; }
        public bool IsSnow
        {
            get
            {
                return _isSnow;
            }
        }

        public Player Controller { get; private set; }

        public Player Owner { get; }

        private Card(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary, bool isSnow)
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

        // Land Constructor
        public Card(Player owner, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary, bool isSnow) : this(owner, false, null, types, subtypes, isBasic, isLegendary, isSnow)
        {
        }

        // Spell Constructor
        public Card(Player owner, CardType[] types, string[] subtypes, bool isLegendary) : this(owner, true, null, types, subtypes, false, isLegendary, false)
        {
        }

        // Non-Land Permanent Constructor
        public Card(Player owner, CardType[] types, string[] subtypes, bool isLegendary, bool isSnow) : this(owner, true, null, types, subtypes, false, isLegendary, isSnow)
        {
        }

        // Creature Constructor
        public Card(Player owner, CardType[] types, string[] subtypes, bool isBasic, int basePower, int baseToughness, bool isLegendary, bool isSnow) : this(owner, true, null, types, subtypes, false, isLegendary, isSnow)
        {
            _basePower = basePower;
            _baseToughness = baseToughness;
        }

        public Func<Game, bool> CanCast = (Game game) => { return true; };

        public Action<Game> OnCast = (game) => { };

        /// <summary>
        /// The method that is called as the spell resolves. If a spell is exiled after it resolves, exile it in this method
        /// </summary>
        /// <param name="game"></param>
        public Action<Game> OnResolve = (game) => { };

        public void GiveControl(Player player)
        {
            Controller = player;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
