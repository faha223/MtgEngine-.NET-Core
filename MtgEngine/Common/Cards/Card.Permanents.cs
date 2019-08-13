using MtgEngine.Common.Abilities;
using MtgEngine.Common.Enums;
using System;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        private List<Ability> _abilities { get; } = new List<Ability>();
        public List<Ability> Abilities
        {
            get
            {
                return _abilities;
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
