using MtgEngine.Common.Abilities;
using MtgEngine.Common.Damage;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        public Player DefendingPlayer { get; set; } = null;

        public bool IsAttacking { get { return DefendingPlayer != null; } }

        public Card Blocking { get; set; } = null;

        public List<DamageInfo> DamageAccumulated { get; private set; } = new List<DamageInfo>();

        public event TookDamageEventHandler TookDamage;

        public void TakeDamage(int amount, Card source)
        { 
            TookDamage?.Invoke(this, source, amount);

            if (IsAPlaneswalker && !IsACreature)
            {
                RemoveCounters(source, amount, CounterType.Loyalty);
            }
            else
            {
                if (source.HasInfect)
                    AddCounters(source, amount, CounterType.Minus1Minus1);
                else
                {
                    DamageAccumulated.Add(new DamageInfo(source, amount));
                }
            }
        }

        public bool IsDead
        {
            get
            {
                int damageAccumulated = DamageAccumulated.Sum(c => c.DamageAmount);
                if(IsACreature)
                {
                    // If toughness goes below 0, then dead
                    if (Toughness <= 0)
                        return true;

                    // If Indestructible, then cannot be destroyed by damage
                    if (!HasIndestructible)
                    {
                        if (damageAccumulated >= Toughness)
                            return true;

                        // Check all the damage sources and see if any come from a card that has deathtouch
                        foreach(var damageSource in DamageAccumulated.Where(c => c.DamageAmount >= 1).Select(c => c.DamageSource))
                        {
                            Card sourceCard = null;

                            if(damageSource is Card)
                                sourceCard = damageSource as Card;
                            else if(damageSource is Ability)
                                sourceCard = (damageSource as Ability).Source;

                            if (sourceCard != null && sourceCard.HasDeathtouch)
                                return true;
                        }
                    }
                }
                else if(IsAPlaneswalker)
                {
                    return Counters.Count(c => c == CounterType.Loyalty) <= 0;
                }
                return false;
                
            }
        }
            

        public void ResetDamage()
        {
            DamageAccumulated.Clear();
        }

        private Func<bool> canAttackAsThoughItDidntHaveDefender = () => { return false; };

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

        public bool CanBlock(Card permanent)
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
                if (Controller.Battlefield.Lands.Any(c => c.SubtypesAfterModifiersApplied.Contains("Plains")))
                    return false;
            }

            // Creatures with Islandwalk can't be blocked if we control a Island
            if (permanent.HasIslandwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.SubtypesAfterModifiersApplied.Contains("Island")))
                    return false;
            }

            // Creatures with Swampwalk can't be blocked if we control a Swamp
            if (permanent.HasSwampwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.SubtypesAfterModifiersApplied.Contains("Swamp")))
                    return false;
            }

            // Creatures with Mountainwalk can't be blocked if we control a Mountain
            if (permanent.HasMountainwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.SubtypesAfterModifiersApplied.Contains("Mountain")))
                    return false;
            }

            // Creatures with Forestwalk can't be blocked if we control a Forest
            if (permanent.HasForestwalk)
            {
                if (Controller.Battlefield.Lands.Any(c => c.SubtypesAfterModifiersApplied.Contains("Forest")))
                    return false;
            }

            #endregion Land-walk

            #endregion Evasion Abilities

            // This creature can block that creature. It passed all the tests
            return true;
        }
    }
}
