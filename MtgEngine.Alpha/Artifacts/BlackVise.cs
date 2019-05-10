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
    public class BlackVise : Card
    {
        public Player chosenOpponent;

        public BlackVise(Player owner) : base(owner, new[] { CardType.Artifact }, null, false, false, false)
        {
            Cost = ManaCost.Parse(this, "{1}");

            Abilities.Add(new BlackViseAbility(this));
        }

        public override void OnResolve(Game game)
        {
            Controller.ChoosePlayer("As Black Vise enters the battlefield, choose an opponent.", game.Players().Except(new[] { Controller }));
        }

        public class BlackViseAbility : EventTriggeredAbility
        {
            public BlackViseAbility(Card source) : base(source, "At the beginning of the chosen player’s upkeep, Black Vise deals X damage to that player, where X is the number of cards in their hand minus 4.")
            {
            }

            public override Ability Copy(Card newSource)
            {
                throw new NotImplementedException();
            }

            public override void OnResolve(Game game)
            {
                Player opponent = (Source as BlackVise).chosenOpponent;
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
