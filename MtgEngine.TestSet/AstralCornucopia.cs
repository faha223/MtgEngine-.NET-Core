﻿using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Astral Cornucopia", "TestSet", "", "", "Astral Cornucopia enters the battlefield with X charge counters on it.\n{T}: Choose a color. Add one mana of that color for each charge counter on Astral Cornucopia.")]
    public class AstralCornucopia : ArtifactCard
    {
        private int _x;

        public AstralCornucopia(Player owner) : base(owner, true, null, new[] { CardType.Artifact }, null, false, false)
        {
            Cost = ManaCost.Parse(this, "{X}{X}{X}");

            // Subscribe to X event so that we know what the value for X was when this enters the battlefield
            var mc = Cost as ManaCost;
            mc.ValueforXChosen += (x => _x = x);
        }
    }
}