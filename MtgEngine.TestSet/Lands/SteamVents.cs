using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.TestSet.Lands
{
    [MtgCard("Steam Vents", "TestSet", "", "", "As Steam Vents enters the battlefield, you may pay 2 life. If you don't, it enters the battlefield tapped.\n{T}: Add {U} or {R}.")]
    public class SteamVents : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, new[] { "Island", "Mountain" }, false);
            card._attrs = CardAttrs;

            // As Steam Vents enters the battlefield, you may pay 2 life. If you don't, it enters the battlefield tapped.
            card.OnResolve = (g, c) =>
            {
                var options = new List<string>(new[] { "yes", "no" });
                var choice = c.Controller.MakeChoice("As Steam Vents enters the battlefield, you may pay 2 life. If you don't, it enters the battlefield tapped.\nDo you want to pay 2 life?", 1, options);
                if(choice[0] == 0)
                {
                    c.Controller.LoseLife(2, c);
                }
                else
                {
                    c.Tap();
                }
            };

            card.AddAbility(new ManaAbility(card, new TapCost(card), new ManaAmount(1, ManaColor.Blue), "{T}: Add {U}."));
            card.AddAbility(new ManaAbility(card, new TapCost(card), new ManaAmount(1, ManaColor.Red), "{T}: Add {R}."));

            return card;
        }
    }
}
