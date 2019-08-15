using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System.Linq;

namespace MtgEngine.TestSet.Snow_Covered_Lands
{
    [MtgCard("Dark Depths", "TestSet", "", "", "Dark Depths enters the battlefield with ten ice counters on it.\n{3}: Remove an ice counter from Dark Depths.\nWhen Dark Depths has no ice counters on it, sacrifice it.If you do, create Marit Lage, a legendary 20/20 black Avatar creature token with flying and indestructible.")]
    public class DarkDepths : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, null, false, true, true);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            // Dark Depths enters the battlefield with 10 ice counters on it
            card.AddCounters(card, 10, CounterType.Ice);

            // {3}: Remove an ice counter from Dark Depths
            card.AddAbility(new DarkDepthsAbility(card));

            // When Dark Depths has no ice counters on it, sacrifice it. If you do, create Marit Lage, a legendary 20/20 black Avatar creature token with flying and indestructible
            card.AddAbility(new MaritLageAbility(card));

            return card;
        }

        public class DarkDepthsAbility : ActivatedAbility
        {
            public DarkDepthsAbility(Card source) : base(source, ManaCost.Parse(source, "{3}"), "{3}: Remove an ice counter from Dark Depths")
            {
            }

            public override void OnResolve(Game game)
            {
                Source.RemoveCounters(this, 1, CounterType.Ice);
            }

            public override Ability Copy(Card newSource)
            {
                return new DarkDepthsAbility(newSource);
            }
        }

        public class MaritLageAbility : StateTriggeredAbility
        {
            public MaritLageAbility(Card source) : base(source, "When Dark Depths has no ice counters on it, sacrifice it. If you do, create Marit Lage, a legendary 20/20 black Avatar creature token with flying and indestructible")
            {
            }

            public override bool CheckState(Game game)
            {
                return 
                    !game.AbilitiesOnStack().Contains(this) &&              // Don't put this on the stack if it's already on the stack
                    Source.Controller.Battlefield.Contains(Source) &&       // Don't put this on the stack if Dark Depths is no longer on the battlefield
                    Source.Counters.Count(c => c == CounterType.Ice) == 0;  // Don't put this on the stack if Dark Depths still has ice counters
            }

            public override void OnResolve(Game game)
            {
                var controller = Source.Controller;

                // If the player was unable to sacrifice the Dark Depths when
                if (controller.Sacrifice(Source))
                {
                    game.PutPermanentOnBattlefield(new MaritLage().GetCard(controller));
                }
            }

            public override Ability Copy(Card newSource)
            {
                return new MaritLageAbility(newSource);
            }
        }

        [MtgCard("Marit Lage", "TestSet", "", "")]
        public class MaritLage : CardSource
        {
            public override Card GetCard(Player owner)
            {
                var card = new Card(owner, new[] { CardType.Creature, CardType.Token }, new[] { "Avatar" }, false, 20, 20, true, false);
                card._attrs = MtgCardAttribute.GetAttribute(GetType());

                card.StaticAbilities.Add(StaticAbility.Flying);
                card.StaticAbilities.Add(StaticAbility.Indestructible);

                // Override Color Identity to Black
                card.Modifiers.Add(new ColorModifier(card, nameof(Card.ColorIdentity), ModifierMode.Override, ManaColor.Black));

                return card;
            }
        }
    }
}
