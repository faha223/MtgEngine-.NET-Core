using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Camouflage", "LEA", "", "", "Cast this spell only during your declare attackers step.\nThis turn, instead of declaring blockers, each defending player chooses any number of creatures they control and divides them into a number of piles equal to the number of attacking creatures for whom that player is the defending player.Creatures those players control that can block additional creatures may likewise be put into additional piles.Assign each pile to a different one of those attacking creatures at random.Each creature in a pile that can block the creature that pile is assigned to does so. (Piles can be empty.)")]
    public class Camouflage : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Instant }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{G}");

            card.CanCast = (g, c) =>
            {
                return (g.ActivePlayer == c.Controller && g.CurrentStep == Phases.DeclareAttackers);
            };

            // TODO: This turn, instead of declaring blockers, each defending player chooses any number of creatures
            // they control and divides them into a number of piles equal to the number of attacking creatures for
            // whom that player is the defending player. Creatures those players control that can block additional
            // creatures may likewise be put into additional piles. Assign each pile to a different one of those
            // attacking creatures at random. Each creature in a pile that can block the creature that pile is
            // assigned to does so. (Piles can be empty.)

            return card;
        }
    }
}
