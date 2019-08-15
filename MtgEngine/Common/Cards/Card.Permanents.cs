using MtgEngine.Common.Abilities;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        private List<Effect> _effects { get; } = new List<Effect>();

        public void AddEffect(Effect effect)
        {
            _effects.Add(effect);
        }

        public ReadOnlyCollection<Effect> Effects
        {
            get
            {
                var effects = new List<Effect>(_effects);
                if(Modifiers.Any(c => c.Property == nameof(Effects)))
                {
                    foreach(EffectModifier modifier in Modifiers.Where(c => c.Property == nameof(Effects)))
                    {
                        if(modifier.Mode == ModifierMode.Add)
                        {
                            if (!effects.Contains(modifier.Value))
                                effects.Add(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Remove)
                        {
                            if (effects.Contains(modifier.Value))
                                effects.Remove(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Override)
                        {
                            effects.Clear();
                            if (modifier.Value != null)
                                effects.Add(modifier.Value);
                        }
                    }
                }

                return effects.AsReadOnly();
            }
        }

        private List<Ability> _abilities { get; } = new List<Ability>();
        
        public void AddAbility(Ability ability)
        {
            _abilities.Add(ability);
        }

        public ReadOnlyCollection<Ability> Abilities
        {
            get
            {
                var abilities = new List<Ability>(_abilities);
                if(Modifiers.Any(c => c.Property == nameof(Abilities)))
                {
                    foreach(AbilityModifier modifier in Modifiers.Where(c => c.Property == nameof(Abilities)))
                    {
                        if (modifier.Mode == ModifierMode.Add)
                        {
                            abilities.Add(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Remove)
                        {
                            abilities.Remove(modifier.Value);
                        }
                        else if(modifier.Mode == ModifierMode.Override)
                        {
                            abilities.RemoveAll(c => true);
                            if(modifier.Value != null)
                                abilities.Add(modifier.Value);
                        }
                    }
                }
                return abilities.AsReadOnly();
            }
        }

        public bool CanBeTargetedBy(IResolvable other)
        {
            // Permanents with Shroud cannot be targeted by spells
            if (StaticAbilities.Contains(StaticAbility.Shroud))
                return false;

            if (other is Card)
            {
                var ctrl = (other as Card).Controller;

                // Permanents with Hexproof can't be targeted other players' spells.
                if (StaticAbilities.Contains(StaticAbility.Hexproof))
                {
                    if (ctrl != Controller)
                        return false;
                }
            }
            return true;
        }

        public bool IsTapped { get; private set; }

        public Func<bool> UntapsDuringUntapStep = () => { return true; };

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
    }
}
