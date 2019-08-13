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
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{U}");

            card.OnCast = (game) =>
            {
                var _target = card.Controller.ChooseTarget(card, new List<ITarget>(game.Players())) as Player;
                card.SetVar("Target", _target);
            };

            card.OnResolve = (game) =>
            {
                var _target = card.GetVar<Player>("Target");
                _target.Draw(3);
            };

            return card;
        }
    }
}
