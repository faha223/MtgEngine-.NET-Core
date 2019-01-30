using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Black Knight", "LEA", "", "", FlavorText = "Battle doesn’t need a purpose; the battle is its own purpose. You don’t ask why a plague spreads or a field burns. Don’t ask why I fight.")]
    public class BlackKnight : PermanentCard
    {
        public BlackKnight(Player owner) : base(owner, true, null, new[] { CardType.Creature }, new[] { "Human", "Knight" }, false, 2, 2, false, false)
        {
            Cost = ManaCost.Parse(this, "{B}{B}");

            //TODO: StaticAbilities.Add(StaticAbility.ProtectionFromWhite);
            StaticAbilities.Add(StaticAbility.FirstStrike);
        }
    }
}
