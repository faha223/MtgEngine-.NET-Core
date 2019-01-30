using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Berserk", "LEA", "", "", Text = "Cast this spell only before the combat damage step.\nTarget creature gains trample and gets +X/+0 where X is its power.\nAt the beginning of the next end step, destroy this creature if it attacked this turn.")]
    public class Berserk : SpellCard
    {
        PermanentCard target;

        public Berserk(Player owner) : base(owner, null, new[] { CardType.Instant }, null, false)
        {
            Cost = ManaCost.Parse(this, "{G}");

            // TODO: Target creature gains trample and gets +X/+0 until end of turn where X is its power.

            // TODO: At the beginning of the next end step, destroy that creature if it attacked this turn.
        }

        public override void OnCast(Game game)
        {
            target = Controller.SelectTarget("Select Target Creature", (card) => card.IsACreature) as PermanentCard;
        }

        public override bool CanCast(Game game)
        {
            // TODO: Cast this spell only before the combat damage step
            return true;
        }
    }
}
