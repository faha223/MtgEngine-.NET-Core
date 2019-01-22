﻿using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Scrubland", "LEA", "", "", "({T}: Add {W} or {B})")]
    public class Scrubland : LandCard
    {
        public Scrubland(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Plains", "Swamp" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {W}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {B}"));
        }
    }
}
