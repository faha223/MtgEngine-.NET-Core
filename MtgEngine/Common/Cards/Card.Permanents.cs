using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    public abstract partial class Card : IDamageable
    {
        protected List<Ability> _copiedCardAbilities;
        protected List<Ability> _abilities { get; } = new List<Ability>();
        public virtual List<Ability> Abilities
        {
            get
            {
                if (_copiedCardAbilities != null)
                    return _copiedCardAbilities;
                return _abilities;
            }
        }

        public virtual bool CanBeTargetedBy(IResolvable other)
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

        public bool IsTapped { get; protected set; }

        public virtual bool UntapsDuringUntapStep
        {
            get
            {
                // TODO: Check for Modifiers that would make this false
                return true;
            }
        }

        public virtual void Untap()
        {
            if (IsTapped)
                IsTapped = false;
        }

        public virtual void Tap()
        {
            if (!IsTapped)
                IsTapped = true;
        }
    }
}
