using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtgEngine.TestSet
{
    [MtgCard("Grizzly Bears", "TestSet", "", "")]
    public class GrizzlyBears : CreatureCard
    {
        public GrizzlyBears(Player owner) : base(owner, null, new CardType[] { CardType.Creature }, new string[] { "Bear" }, 2, 2, false, false)
        {
            Cost = ManaCost.Parse(this, "{1}{G}");
        }
    }
}
