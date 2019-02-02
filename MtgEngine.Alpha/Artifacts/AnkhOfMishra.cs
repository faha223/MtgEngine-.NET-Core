using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    // TODO: Add a continuous effect that subscribes to Enters the Battlefield events
    [MtgCard("Ankh of Mishra", "LEA", "", "", Text = "Whenever a land enters the battlefield, Ankh of Mishra deals 2 damage to that land's controller.")]
    public class AnkhOfMishra : ArtifactCard
    {
        public AnkhOfMishra(Player owner) : base(owner, true, null, new[] { CardType.Artifact }, null, false, false)
        {
            Cost = ManaCost.Parse(this, "{2}");
        }

        internal class AnkhOfMishraAbility : EventTriggeredAbility
        {
            Player player;

            public AnkhOfMishraAbility(PermanentCard source, string text) : base(source, text)
            {
            }

            // Whenever a land enters the battlefield, Ankh of Mishra deals 2 damage to that land’s controller.

            public override void CardHasChangedZones(Game game, Card card, Zone previousZone, Zone currentZone)
            {
                if(currentZone == Zone.Battlefield && card.IsALand)
                {
                    player = card.Controller;
                    game.AbilityTriggered(this);
                }
            }

            public override void OnResolve(Game game)
            {
                if (player != null)
                    game.ApplyDamage(player, Source, 2);
            }
        }
    }
}
