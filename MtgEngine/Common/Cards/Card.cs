﻿using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        private void ApplyActiveEffects()
        {
            if (Controller.Game != null)
            {
                foreach (var effect in Controller.Game.ActiveEffects)
                {
                    effect.ModifyObject(Controller.Game, this);
                }
            }
        }

        private void UnApplyActiveEffects()
        {
            if (Controller.Game != null)
            {
                foreach (var effect in Controller.Game.ActiveEffects)
                {
                    effect.UnmodifyObject(Controller.Game, this);
                }
            }
        }

        private List<StaticAbility> _staticAbilities { get; } = new List<StaticAbility>();
        public ReadOnlyCollection<StaticAbility> StaticAbilities
        {
            get
            {
                var abilities = new List<StaticAbility>(_staticAbilities);
                if (Modifiers.Any(c => c.Property == nameof(StaticAbilities)))
                {
                    foreach (StaticAbilityModifier modifier in Modifiers.Where(c => c.Property == nameof(StaticAbilities)))
                    {
                        if (modifier.Mode == ModifierMode.Add)
                        {
                            if (!abilities.Contains(modifier.Value.Value))
                                abilities.Add(modifier.Value.Value);
                        }
                        else if (modifier.Mode == ModifierMode.Remove)
                        {
                            if (abilities.Contains(modifier.Value.Value))
                                abilities.Remove(modifier.Value.Value);
                        }
                        else if (modifier.Mode == ModifierMode.Override)
                        {
                            abilities.Clear();
                            if (modifier.Value.HasValue)
                                abilities.Add(modifier.Value.Value);
                        }
                    }
                }
                return new ReadOnlyCollection<StaticAbility>(abilities);
            }
        }

        public void AddStaticAbility(StaticAbility ability)
        {
            _staticAbilities.Add(ability);
        }

        public Guid InstanceId { get; } = Guid.NewGuid();

        private Cost _cost { get; set; }
        public Cost Cost
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(Cost)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(Cost)) as CostModifier;
                    return modifier.Value;
                }
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }

        private CardType[] _types { get; set; }
        public CardType[] Types
        {
            get
            {
                var type = new List<CardType>(_types);
                if (Modifiers.Any(c => c.Property == nameof(Types)))
                {
                    foreach (CardTypeModifier modifier in Modifiers.Where(c => c.Property == nameof(Types)))
                    {
                        if (modifier.Mode == ModifierMode.Add)
                        {
                            if (!type.Contains(modifier.Value.Value))
                                type.Add(modifier.Value.Value);
                        }
                        else if (modifier.Mode == ModifierMode.Remove)
                        {
                            if (type.Contains(modifier.Value.Value))
                                type.Remove(modifier.Value.Value);
                        }
                        else if (modifier.Mode == ModifierMode.Override)
                        {
                            type.Clear();
                            if (modifier.Value.HasValue)
                                type.Add(modifier.Value.Value);
                        }
                    }
                }
                return type.ToArray();
            }
        }

        // This is virtual so that it can be overridden in token classes
        private ManaColor[] _colorIdentity
        {
            get
            {
                if (StaticAbilities.Contains(StaticAbility.Devoid))
                    return new[] { ManaColor.Colorless };

                if (Cost is ManaCost)
                    return (Cost as ManaCost).Colors;

                return new[] { ManaColor.Colorless };
            }
        }

        public ManaColor[] ColorIdentity
        {
            get
            {
                var colors = new List<ManaColor>(_colorIdentity);
                if (Modifiers.Any(c => c.Property == nameof(ColorIdentity)))
                {
                    foreach(ColorModifier modifier in Modifiers.Where(c => c.Property == nameof(ColorIdentity)))
                    {
                        if(modifier.Mode == ModifierMode.Add)
                        {
                            if (!colors.Contains(modifier.Value))
                                colors.Add(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Remove)
                        {
                            if (colors.Contains(modifier.Value))
                                colors.Remove(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Override)
                        {
                            colors.Clear();
                            colors.Add(modifier.Value);
                        }
                    }

                    if (colors.Count > 1 && colors.Contains(ManaColor.Colorless))
                        colors.Remove(ManaColor.Colorless);
                }

                return colors.ToArray();
            }
        }

        private string[] _subtypes { get; }
        public string[] Subtypes
        {
            get
            {
                var subtypes = new List<string>(_subtypes ?? new string[] { });
                if (Modifiers.Any(c => c.Property == nameof(Subtypes)))
                {
                    foreach(StringModifier modifier in Modifiers.Where(c => c.Property == nameof(Subtypes)))
                    {
                        if(modifier.Mode == ModifierMode.Add)
                        {
                            if (!subtypes.Contains(modifier.Value))
                                subtypes.Add(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Remove)
                        {
                            if (subtypes.Contains(modifier.Value))
                                subtypes.Remove(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Override)
                        {
                            subtypes.Clear();
                            if (modifier.Value != null)
                                subtypes.Add(modifier.Value);
                        }
                    }
                }

                return subtypes.ToArray();
            }
        }

        private bool _isBasic { get; }
        public bool IsBasic
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(IsBasic)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(IsBasic)) as BooleanModifier;
                    return modifier.Value;
                }
                return _isBasic;
            }
        }

        private bool _isLegendary { get; }
        public bool IsLegendary
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(IsLegendary)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(IsLegendary)) as BooleanModifier;
                    return modifier.Value;
                }
                return _isLegendary;
            }
        }

        private bool _isSnow { get; }
        public bool IsSnow
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(IsSnow)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(IsSnow)) as BooleanModifier;
                    return modifier.Value;
                }
                return _isSnow;
            }
        }

        private bool _canBeCountered { get; set; } = true;
        public bool CanBeCountered
        {
            get
            {
                if(Modifiers.Any(c => c.Property == nameof(CanBeCountered)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(CanBeCountered)) as BooleanModifier;
                    return modifier.Value;
                }
                return _canBeCountered;
            }
        }

        public void SetCanBeCountered(bool canBeCountered)
        {
            _canBeCountered = canBeCountered;
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
            BasePowerFunc = (game, card) => { return basePower; };
            BaseToughnessFunc = (game, card) => { return baseToughness; };
        }

        // Creature Constructor, functional power toughness
        public Card(Player owner, CardType[] types, string[] subtypes, bool isBasic, Func<Game, Card, int> basePowerFunc, Func<Game, Card, int> baseToughnessFunc, bool isLegendary, bool isSnow) : this(owner, true, null, types, subtypes, false, isLegendary, isSnow)
        {
            BasePowerFunc = basePowerFunc;
            BaseToughnessFunc = baseToughnessFunc;
        }

        // Copy constructor
        public Card(Player owner, Card original, bool isToken)
        {
            Controller = owner;
            _attrs = original._attrs;
            _cost = original.Cost.Copy(this);
            _types = new List<CardType>(original.Types).ToArray();
            if(isToken)
            {
                var t = new List<CardType>(_types);
                t.Add(CardType.Token);
                _types = t.ToArray();
            }
            _subtypes = new List<string>(original.Subtypes).ToArray();
            _staticAbilities = new List<StaticAbility>(original.StaticAbilities);
            _isBasic = original._isBasic;
            _isLegendary = original._isLegendary;
            _isSnow = original._isSnow;
            UsesStack = original.UsesStack;

            foreach(var ability in original.Abilities)
            {
                _abilities.Add(ability.Copy(this));
            }

            // Add starting loyalty counters
            _startingLoyalty = original._startingLoyalty;
            AddCounters(this, _startingLoyalty, CounterType.Loyalty);
            BasePowerFunc = original.BasePowerFunc;
            BaseToughnessFunc = original.BaseToughnessFunc;
        }

        public Func<Game, Card, bool> CanCast = (game, card) => { return true; };

        public Action<Game, Card> OnCast;

        /// <summary>
        /// The method that is called as the spell resolves. If a spell is exiled after it resolves, exile it in this method
        /// </summary>
        /// <param name="game"></param>
        public Action<Game, Card> OnResolve;

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
