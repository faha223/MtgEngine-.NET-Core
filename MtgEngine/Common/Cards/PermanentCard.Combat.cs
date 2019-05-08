using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public abstract partial class PermanentCard : Card, IDamageable
    {
        public Player DefendingPlayer { get; set; } = null;

        public bool IsAttacking { get { return DefendingPlayer != null; } }

        public Card Blocking { get; set; } = null;

        public int DamageAccumulated { get; private set; } = 0;

        public event TookDamageEventHandler TookDamage;

        public virtual void TakeDamage(int amount, Card source)
        { 
            TookDamage?.Invoke(this, source, amount);

            if((source is PermanentCard) && (source as PermanentCard).HasInfect)
                AddCounters(source, amount, CounterType.Minus1Minus1);
            else
                DamageAccumulated += amount;
        }

        public virtual bool IsDead => Toughness <= 0 || (DamageAccumulated >= Toughness && !HasIndestructible);

        public void ResetDamage()
        {
            DamageAccumulated = 0;
        }

        protected virtual bool canAttackAsThoughItDidntHaveDefender() => false;

        public bool CanAttack
        {
            get
            {
                if (!IsACreature)
                    return false;
                if (IsTapped)
                    return false;
                if (HasDefender && !canAttackAsThoughItDidntHaveDefender())
                    return false;
                if (IsTapped)
                    return false;
                if (HasSummoningSickness && !HasHaste)
                    return false;
                return true;
            }
        }

        public bool CanBlock(PermanentCard permanent)
        {
            // This permanent can't block if it is not a creature (Vehicles can only attack/block if they're creatures)
            if (!IsACreature)
                return false;

            // Can't block non-creatures (Vehicles can only attack/block if they're creatures)
            if (!permanent.IsACreature)
                return false;

            #region Evasion Abilities

            #region Unblockable

            // Can't block unblockables. Seems pretty straightforward
            if (permanent.HasUnblockable)
                return false;

            #endregion Unblockable

            #region Fear

            // Creatures with Fear can only be blocked by artifacts and black creatures
            if (permanent.HasFear)
            {
                if (!IsAnArtifact || ColorIdentity == null || ColorIdentity.Contains(ManaColor.Black))
                    return false;
            }

            #endregion Fear

            #region Shadow

            // Only creatures with shadow can block attackers with shadow
            if (permanent.HasShadow)
            {
                if (!HasShadow)
                    return false;
            }

            #endregion Shadow

            #region Horsemanship

            // Only creatures with horsemanship can block attackers with horsemanship
            if (permanent.HasHorsemanship)
            {
                if (!HasHorsemanship)
                    return false;
            }

            #endregion Horsemanship

            #region Flying

            // Only creatures with Flying or Reach can block creatures with flying
            if (permanent.HasFlying)
            {
                if (!HasFlying && !HasReach)
                    return false;
            }

            #endregion Flying

            #region Land-walk

            // Creatures with Plainswalk can't be blocked if we control a Plains
            if (permanent.HasPlainswalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.Subtypes.Contains("Plains")))
                    return false;
            }

            // Creatures with Islandwalk can't be blocked if we control a Island
            if (permanent.HasIslandwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.Subtypes.Contains("Island")))
                    return false;
            }

            // Creatures with Swampwalk can't be blocked if we control a Swamp
            if (permanent.HasSwampwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.Subtypes.Contains("Swamp")))
                    return false;
            }

            // Creatures with Mountainwalk can't be blocked if we control a Mountain
            if (permanent.HasMountainwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.Subtypes.Contains("Mountain")))
                    return false;
            }

            // Creatures with Forestwalk can't be blocked if we control a Forest
            if (permanent.HasForestwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.Subtypes.Contains("Forest")))
                    return false;
            }

            #endregion Land-walk

            #endregion Evasion Abilities

            // This creature can block that creature. It passed all the tests
            return true;
        }
    }
}
