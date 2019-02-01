using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;

namespace MtgEngine.TestSet
{
    [MtgCard("Counterspell", "TestSet", "", "", "Counter target spell")]
    public class Counterspell : SpellCard
    {
        Card target;

        public Counterspell(Player owner) : 
            base(owner, null, new[] { CardType.Instant }, null, false)
        {
            Cost = ManaCost.Parse(this, "{U}{U}");
        }

        public override bool CanCast(Game game)
        {
            // Can't cast if there are no targets
            var possibleTargets = game.CardsOnStack();
            if (possibleTargets.Count == 0)
                return false;

            return base.CanCast(game);
        }

        public override void OnCast(Game game)
        {
            var possibleTargets = game.CardsOnStack();
            if (possibleTargets.Count == 0)
                throw new InvalidOperationException("Counterspell can't be cast if there are no spells on the stack");
            target = Controller.ChooseTarget(this, new List<ITarget>(possibleTargets)) as Card;
        }

        public override void OnResolve(Game game)
        {
            game.Counter(target);
        }
    }
}
