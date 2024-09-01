using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Blue Elemental Blast", "LEA", "", "", "Choose one —\n• Counter target red spell.\n• Destroy target red permanent.")]
    public class BlueElementalBlast : CardSource
    {
        private static string[] modes =
        {
            "Counter target red spell",
            "Destroy target red permanent"
        };

        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Instant }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{U}");

            card.OnCast = (g, c) =>
            {
                var options = new List<string>(modes);
                var choice = c.Controller.MakeChoice("Choose One", 1, options);
                c.SetVar("Mode", choice[0]);
                if(choice[0] == 1)
                {
                    // Select target red spell
                    var target = c.Controller.ChooseTarget(c, new List<ITarget>(g.CardsOnStack().Where(c2 => c2.IsRed && c2.CanBeTargetedBy(c))), "Choose target red spell") as Card;
                    c.SetVar("Target", target);
                }
                else
                {
                    // Select target red permanent
                    var target = c.Controller.SelectTarget("Select target red permanent", c2 => c2.IsRed && c2.CanBeTargetedBy(c)) as Card;
                    c.SetVar("Target", target);
                }
            };

            card.OnResolve = (g, c) =>
            {
                var mode = c.GetVar<string>("Mode");
                var target = c.GetVar<Card>("Target");
                if (mode == modes[0])
                {
                    // Counter target spell
                    g.Counter(target);
                }
                else
                {
                    // Destroy target permanent
                    g.DestroyPermanent(target);
                }
            };

            return card;
        }
    }
}
