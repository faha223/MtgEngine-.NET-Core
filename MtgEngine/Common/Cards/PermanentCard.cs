using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Counters;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public abstract class PermanentCard : Card, IDamageable
    {
        protected int _basePower { get; }
        public virtual int BasePower
        {
            get { return _basePower; }
        }

        public int Power
        {
            get
            {
                return BasePower
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        protected int _baseToughness { get; }
        public virtual int BaseToughness
        {
            get { return _baseToughness; }
        }

        public int Toughness
        {
            get
            {
                return BaseToughness
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        public bool HasSummoningSickness { get; set; } = false;

        public List<Ability> Abilities { get; } = new List<Ability>();

        public List<StaticAbility> StaticAbilities { get; } = new List<StaticAbility>();

        // This isn't protected because we don't want inheriting classes modifying how counters are added or removed
        private List<CounterType> counters { get; } = new List<CounterType>();

        // This returns a copy so that this can't be used to modify the counters
        public ReadOnlyCollection<CounterType> Counters { get { return new ReadOnlyCollection<CounterType>(counters); } }

        public void AddCounters(int count, CounterType counter)
        {
            for(int i = 0; i < count; i++)
            {
                switch(counter)
                {
                    case CounterType.Plus1Plus1:
                        if (counters.Contains(CounterType.Minus1Minus1))
                            counters.Remove(CounterType.Minus1Minus1);
                        else
                            counters.Add(CounterType.Plus1Plus1);
                        break;
                    case CounterType.Minus1Minus1:
                        if (counters.Contains(CounterType.Plus1Plus1))
                            counters.Remove(CounterType.Plus1Plus1);
                        else
                            counters.Add(CounterType.Minus1Minus1);
                        break;
                    default:
                        counters.Add(counter);
                        break;
                }
            }
        }

        public void RemoveCounters(int amount, CounterType counter)
        {
            for(int i = 0; i < amount; i++)
            {
                if (counters.Contains(counter))
                    counters.Remove(counter);
            }
        }

        /// <summary>
        /// This stuff is here instead of on the Permanent class because enchantments and artifacts and lands can become creatures
        /// </summary>
        #region Combat Stuff

        public Player DefendingPlayer { get; set; } = null;

        public bool IsAttacking { get { return DefendingPlayer != null; } }

        public Card Blocking { get; set; } = null;

        public int DamageAccumulated { get; private set; } = 0;

        public virtual void TakeDamage(int amount, Card source)
        {
            DamageAccumulated += amount;
        }

        public virtual bool IsDead => DamageAccumulated >= Toughness;

        public void ResetDamage()
        {
            DamageAccumulated = 0;
        }

        public bool CanAttack
        {
            get
            {
                if (!IsACreature)
                    return false;
                if (IsTapped)
                    return false;
                if (StaticAbilities.Contains(StaticAbility.Defender))
                    return false;
                if (StaticAbilities.Contains(StaticAbility.Haste))
                    return true;
                return !(HasSummoningSickness || IsTapped);
            }
        }

        public bool CanBlock(PermanentCard permanent)
        {
            // This permanent can't block if it is not a creature
            if (!IsACreature)
                return false;

            // Can't block non-creatures (this is for Vehicles)
            if (!permanent.IsACreature)
                return false;

            // Only creatures with Flying or Reach can block creatures with flying
            if (permanent.StaticAbilities.Contains(StaticAbility.Flying))
            {
                if (!StaticAbilities.Contains(StaticAbility.Flying) && !StaticAbilities.Contains(StaticAbility.Reach))
                    return false;
            }

            // This creature can block that creature as long as it's a creature
            return true;
        }

        #endregion Combat Stuff

        public PermanentCard(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary, bool isSnow) : 
            base(owner, usesStack, cost, types, subtypes, false, isLegendary, isSnow)
        {
            _basePower = 0;
            _baseToughness = 0;
        }

        public PermanentCard(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, int basePower, int baseToughness, bool isLegendary, bool isSnow) :
            base(owner, usesStack, cost, types, subtypes, false, isLegendary, isSnow)
        {
            _basePower = basePower;
            _baseToughness = baseToughness;
        }
    }
}
