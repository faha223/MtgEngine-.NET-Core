﻿using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    [MtgCard("Swamp", "", "", "")]
    public class Swamp : BasicLandCard
    {
        public Swamp(Player owner) : base(owner, ManaColor.Black, new[] { CardType.Land }, new[] { "Swamp" }, false)
        {
        }
    }
}
