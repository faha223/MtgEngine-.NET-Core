using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    public abstract partial class PermanentCard : Card, IDamageable
    {
        public List<Ability> Abilities { get; } = new List<Ability>();

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

        public override bool CanBeTargetedBy(IResolvable other)
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
    }
}
