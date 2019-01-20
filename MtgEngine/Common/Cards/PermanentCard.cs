using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Counters;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    public abstract class PermanentCard : Card
    {
        protected int _basePower { get; }
        public virtual int BasePower
        {
            get { return _basePower; }
        }

        protected int _baseToughness { get; }
        public virtual int BaseToughness
        {
            get { return _baseToughness; }
        }

        public bool HasSummoningSickness { get; set; } = false;

        public List<Ability> Abilities { get; } = new List<Ability>();

        public List<StaticAbility> StaticAbilities { get; } = new List<StaticAbility>();

        public List<Counter> Counters { get; } = new List<Counter>();

        /// <summary>
        /// This stuff is here instead of on the Permanent class because enchantments and artifacts and lands can become creatures
        /// </summary>
        #region Combat Stuff

        public Player DefendingPlayer { get; set; } = null;

        public bool IsAttacking { get { return DefendingPlayer != null; } }

        public Card Blocking { get; set; } = null;

        public int DamageAccumulated { get; set; } = 0;

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
