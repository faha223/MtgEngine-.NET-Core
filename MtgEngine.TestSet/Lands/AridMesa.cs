using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Linq;

namespace MtgEngine.TestSet.Lands
{
    [MtgCard("Arid Mesa", "TestSet", "", "", "{T}, Pay 1 life, Sacrifice Arid Mesa: Search your library for a Mountain or Plains card and put it onto the battlefield. Then shuffle your library.")]
    public class AridMesa : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, null, false);
            card._attrs = CardAttrs;

            card.AddAbility(new AridMesaAbility(card));

            return card;
        }

        public class AridMesaAbility : TutorAbility
        {
            public AridMesaAbility(Card card) : base(card, 
                new AggregateCost(card, new TapCost(card), new PayLifeCost(card, 1), new SacrificeSourceCost(card)), // Cost
                c => c.Subtypes.Contains("Mountain") || c.Subtypes.Contains("Plains"),                               // Selector
                1,                                                                                                   // Count
                $"{{T}}, Pay 1 life, Sacrifice {card.Name}: Search your library for a Mountain or Plains card and put it onto the battlefield. Then shuffle your library.") // Text
            {

            }

            public override Ability Copy(Card newSource)
            {
                return new AridMesaAbility(newSource);
            }

            public override void OnFetchCompleted(Game game)
            {
                Controller.Battlefield.AddRange(fetchedCards);
            }
        }
    }
}
