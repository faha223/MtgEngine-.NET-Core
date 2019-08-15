using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.TestSet.Lands
{
    [MtgCard("Thespian's Stage", "TestSet", "", "", Text = "{T}: Add {C}\n{2},{T}: Thespian's Stage becomes a copy of target land, except it has this ability.", FlavorText = "Amid rumors of war, the third act of The Absolution of the Guildpact was quickly rewritten as a tragedy.")]
    public class ThespiansStage : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Land }, null, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.AddAbility(new ManaAbility(card, new TapCost(card), new Common.Mana.ManaAmount(1, ManaColor.Colorless), "{T}: Add {C}"));
            card.AddAbility(new CopyAbility(card));

            return card;
        }

        class CopyAbility : ActivatedAbility, ITargeting
        {
            Card targetLand = null;

            public CopyAbility(Card source) : base(source, null, "{2},{T}: Thespian's Stage becomes a copy of target land, except it has this ability.")
            {
                Cost = new AggregateCost(this, ManaCost.Parse(this, "{2}"), new TapCost(source));
            }

            public override Ability Copy(Card newSource)
            {
                return new CopyAbility(newSource);
            }

            public override void OnResolve(Game game)
            {
                // Stop copying whatever else we're copying right now
                Source.StopCopying(Source.IsCopying, this);

                // Copy Target Land
                Source.Copy(targetLand, this);

                // Except it retains this ability
                Source.Modifiers.Add(new AbilityModifier(this, nameof(Card.Abilities), ModifierMode.Add, this));
            }

            public void SelectTargets(Game game)
            {
                List<ITarget> Lands = new List<ITarget>();
                foreach (var player in game.Players())
                    Lands.AddRange(player.Battlefield.Lands);

                targetLand = (Card)Source.Controller.ChooseTarget(this, Lands);
            }
        }
    }
}
