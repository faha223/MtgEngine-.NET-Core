using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Linq;

namespace MtgEngine.Alpha.Artifacts
{
    [MtgCard("Black Vise", "LEA", "", "", Text = "As Black Vise enters the battlefield, choose an opponent.\n\nAt the beginning of the chosen player’s upkeep, Black Vise deals X damage to that player, where X is the number of cards in their hand minus 4.")]
    public class BlackVise : CardSource
    {
        public Player chosenOpponent;

        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact }, null, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}");

            card.AddAbility(new BlackViseAbility(card));

            card.OnResolve = (Game game) =>
            {
                var chosenPlayer = card.Controller.ChoosePlayer("As Black Vise enters the battlefield, choose an opponent.", game.Players().Except(new[] { card.Controller }));
                card.SetVar("Chosen Player", chosenPlayer);
            };

            return card;
        }

        public class BlackViseAbility : EventTriggeredAbility
        {
            public BlackViseAbility(Card source) : base(source, "At the beginning of the chosen player’s upkeep, Black Vise deals X damage to that player, where X is the number of cards in their hand minus 4.")
            {
            }

            public override Ability Copy(Card newSource)
            {
                return new BlackViseAbility(newSource);
            }

            public override void OnResolve(Game game)
            {
                Player opponent = Source.GetVar<Player>("Chosen Opponent");
                if (opponent != null)
                {
                    int cardsInHand = opponent.Hand.Count;
                    int X = cardsInHand - 4;
                    if (X > 0)
                    {
                        opponent.LoseLife(X, Source);
                    }
                }
            }
        }
    }

}
