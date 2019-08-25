using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Linq;

namespace MtgEngine.Alpha.Sorceries
{
    [MtgCard("Braingeyser", "LEA", "", "", "Target player draws X cards.")]
    public class Braingeyser : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Sorcery }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
            card.Cost = ManaCost.Parse(card, "{X}{U}{U}");

            // This must be a new delegate every time. Otherwise we mess up the card reference.
            (card.Cost as ManaCost).ValueforXChosen += (x => card.SetVar("X", x));

            card.OnCast = (g, c) =>
            {
                var target = c.Controller.ChoosePlayer("Choose Target Player", g.Players().Where(p => true));// p.CanBeTargetedBy(c)))
                c.SetVar("Target", target);
            };

            card.OnResolve = (g, c) =>
            {
                var target = c.GetVar<Player>("Target");
                var X = c.GetVar<int>("X");
                target.Draw(X);
            };

            return card;
        }
    }
}
