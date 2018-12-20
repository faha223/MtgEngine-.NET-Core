﻿using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    [MtgCard("Snow Covered Swamp", "", "", "")]
    public class SnowCoveredSwamp : BasicSnowLand
    {
        public SnowCoveredSwamp(Player owner) : base(owner, ManaColor.Black, new[] { CardType.Land }, new[] { "Swamp" })
        {
        }
    }
}