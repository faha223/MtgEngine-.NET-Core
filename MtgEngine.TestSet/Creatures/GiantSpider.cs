﻿using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Giant Spider", "TestSet", "", "", Text = "Reach (This creature can block creatures with flying.)", FlavorText = "\"After everything I’ve survived, it’s hard to be frightened by anything anymore.\"\n-Vivien Reid")]
    public class GiantSpider : PermanentCard
    {
        public GiantSpider(Player owner) : base(owner, true, null, new[] { CardType.Creature }, new[] { "Spider" }, false, 2, 4, false, false)
        {
            Cost = ManaCost.Parse(this, "{3}{G}");
            StaticAbilities.Add(StaticAbility.Reach);
        }
    }
}
