﻿using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Mountain", "", "", "")]
    public class SnowCoveredMountain : BasicSnowLand
    {
        public SnowCoveredMountain(Player owner) : base(owner, ManaColor.Red, new[] { CardType.Land }, new[] { "Mountain" })
        {
        }
    }
}
