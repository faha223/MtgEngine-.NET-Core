﻿using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Mountain", "", "", "")]
    public class Mountain : BasicLandCard
    {
        public Mountain(Player owner) : base(owner, ManaColor.Red, new[] { CardType.Land }, new[] { "Mountain" }, false)
        {
        }
    }
}