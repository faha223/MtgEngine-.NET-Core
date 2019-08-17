using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Blaze of Glory", "LEA", "", "", "Enchant creature\nEnchanted creature has protection from black.This effect doesn't remove Black Ward.")]
    public class BlazeOfGlory : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Instant }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.CanCast = (g, c) =>
            {
                // Cast this spell only during combat before blockers are declared
                return g.CurrentStep == Phases.DeclareAttackers;
            };

            card.OnCast = (g, c) =>
            {
                var creaturesControlledByDefendingPlayers = new List<ITarget>();

                var attackingCreatures = g.Battlefield.Creatures.Where(_c => _c.IsAttacking).ToList();
                var defendingPlayers = attackingCreatures.Select(_c => _c.DefendingPlayer).Distinct().ToList();

                foreach(var player in defendingPlayers)
                {
                    // Add all creatures that player controls that can block any attacking creature that is attacking that player
                    creaturesControlledByDefendingPlayers.AddRange(player.Battlefield.Creatures.Where(_d => attackingCreatures.Any(_c => _c.DefendingPlayer == player && _d.CanBlock(_c))));
                }

                var target = c.Controller.ChooseTarget(c, creaturesControlledByDefendingPlayers) as Card;
                c.SetVar("Target", target);
            };

            // Target creature defending player controls can block any number of creatures this turn. It blocks each attacking creature this turn if able.

            return card;
        }
    }
}
