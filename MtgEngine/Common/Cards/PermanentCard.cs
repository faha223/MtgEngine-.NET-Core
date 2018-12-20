using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Common.Cards
{
    public abstract class PermanentCard : Card
    {
        public List<Ability> Abilities { get; } = new List<Ability>();

        public PermanentCard(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isBasic, bool isLegendary, bool isSnow) : 
            base(owner, usesStack, cost, types, subtypes, false, isLegendary, isSnow)
        {
        }
    }
}
