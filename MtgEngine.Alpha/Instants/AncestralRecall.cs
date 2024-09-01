using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Ancestral Recall", "LEA", "", "", Text = "Target player draws three cards.")]
    public class AncestralRecall : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Instant }, null, false);
            card._attrs = CardAttrs;

            card.Cost = ManaCost.Parse(card, "{U}");

            card.OnCast = (g, c) =>
            {
                var _target = c.Controller.ChooseTarget(c, new List<ITarget>(g.Players())) as Player;
                c.SetVar("Target", _target);
            };

            card.OnResolve = (g, c) =>
            {
                var _target = c.GetVar<Player>("Target");
                _target.Draw(3);
            };

            return card;
        }
    }
}
