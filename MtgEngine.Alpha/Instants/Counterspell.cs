using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Counterspell", "LEA", "", "", "Counter target spell")]
    public class Counterspell : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Instant }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
        
            card.Cost = ManaCost.Parse(card, "{U}{U}");

            card.CanCast = game =>
            {
                var possibleTargets = game.CardsOnStack();
                if (possibleTargets.Count == 0)
                    return false;

                return true;
            };

            card.OnCast = game =>
            {
                var possibleTargets = game.CardsOnStack();
                if (possibleTargets.Count == 0)
                    throw new InvalidOperationException("Counterspell can't be cast if there are no spells on the stack");
                var target = card.Controller.ChooseTarget(card, new List<ITarget>(possibleTargets)) as Card;
                card.SetVar("Target", target);
            };

            card.OnResolve = game =>
            {
                var target = card.GetVar<Card>("Target");
                game.Counter(target);
            };

            return card;
        }
    }
}
