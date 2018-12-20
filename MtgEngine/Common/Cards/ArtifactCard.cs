﻿using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public abstract class ArtifactCard : PermanentCard
    {
        public ArtifactCard(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isLegendary, bool isSnow) : 
            base(owner, usesStack, cost, types, subtypes, false, isLegendary, isSnow)
        {
        }
    }
}
