using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Linq;

namespace MtgEngine.Alpha.Sorceries
{
    [MtgCard("Balance", "LEA", "", "", Text = "Each player chooses a number of lands they control equal to the number of lands controlled by the player who controls the fewest, then sacrifices the rest. Players discard cards and sacrifice creatures the same way.")]
    public class Balance : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Sorcery }, null, false);
            card._attrs = MtgCard;

            card.Cost = ManaCost.Parse(card, "{1}{W}");

            card.OnResolve = (g, c) =>
            {
                balanceLands(g);

                balanceHands(g);

                balanceCreatures(g);
            };

            return card;
        }

        private void balanceLands(Game game)
        {
            // Get the Player with the fewest lands
            Player playerWithFewestLands = game.Players().OrderBy(p => p.Battlefield.Lands.Count()).First();
            int maxLandCount = playerWithFewestLands.Battlefield.Lands.Count();

            foreach (var player in game.Players())
            {
                if(player.Battlefield.Lands.Count() > maxLandCount)
                {
                    // Player chooses maxLandCount lands and sacrifices the rest
                    var keep = player.MakeChoice($"Choose {maxLandCount}, the rest will be sacrificed", maxLandCount, player.Battlefield.Lands.ToList());
                    var sorted = player.Sort("Choose the order you want these to enter the graveyard", player.Battlefield.Lands.Except(keep).ToList());
                    foreach(var card in sorted)
                    {
                        player.Sacrifice(card);
                    }
                }
            }
        }

        private void balanceHands(Game game)
        {
            // Get the Player with the fewest cards in hand
            Player playerWithFewestCardsInHand = game.Players().OrderBy(p => p.Hand.Count()).First();
            int maxHandSize = playerWithFewestCardsInHand.Hand.Count;

            foreach (var player in game.Players())
            {
                if (player.Hand.Count > maxHandSize)
                {
                    // Player chooses maxHandSize cards, and discards the rest
                    var keep = player.MakeChoice($"Choose {maxHandSize}, the rest will be sacrificed", maxHandSize, player.Hand.ToList());
                    var sorted = player.Sort("Choose the order you want these to be discarded", player.Hand.Except(keep).ToList());
                    foreach (var card in sorted)
                    {
                        player.Discard(card);
                    }
                }
            }
        }

        private void balanceCreatures(Game game)
        {
            // Get the Player with the fewest creatures
            Player playerWithFewestCreatures = game.Players().OrderBy(p => p.Battlefield.Creatures.Count()).First();
            int maxCreatureCount = playerWithFewestCreatures.Battlefield.Creatures.Count();

            foreach(var player in game.Players())
            {
                if(player.Battlefield.Creatures.Count() > maxCreatureCount)
                {
                    // Player chooses maxCreatureCount creatures and sacrifices the rest
                    var keep = player.MakeChoice($"Choose {maxCreatureCount}, the rest will be sacrificed", maxCreatureCount, player.Battlefield.Creatures.ToList());
                    var sorted = player.Sort("Choose the order you want these to enter the graveyard", player.Battlefield.Creatures.Except(keep).ToList());
                    foreach (var card in sorted)
                    {
                        player.Sacrifice(card);
                    }
                }
            }
        }
    }
}
