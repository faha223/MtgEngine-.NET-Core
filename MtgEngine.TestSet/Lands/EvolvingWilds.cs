using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Lands
{
    [MtgCard("Evolving Wilds", "TestSet", "", "", "{T}, Sacrifice Evolving Wilds: Search your library for a basic land card, put it onto the battlefield tapped, then shuffle your library.")]
    public class EvolvingWilds : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, null, false);
            card._attrs = CardAttrs;

            card.AddAbility(new EvolvingWildsAbility(card));

            return card;
        }
    }

    public class EvolvingWildsAbility : TutorAbility
    {
        public EvolvingWildsAbility(Card source) : base(source, new TapCost(source), card => card.IsALand && card.IsBasic, 1, 
            $"{{T}}, Sacrifice {source.Name}: Search your library for a basic land card, put it onto the battlefield tapped, then shuffle your library.")
        {

        }

        public override Ability Copy(Card newSource)
        {
            return new EvolvingWildsAbility(newSource);
        }

        public override void OnFetchCompleted(Game game)
        {
            // Fetched cards enter the battlefield tapped
            foreach (var card in fetchedCards)
                card.Tap();

            Controller.Battlefield.AddRange(fetchedCards);
        }
    }
}
