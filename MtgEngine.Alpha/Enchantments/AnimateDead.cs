using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Effects;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Alpha.Enchantments
{
    // TODO: Make this an Aura card that targets creatures in graveyards
    public class AnimateDead : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}{B}");

            // TODO: Enchant creature card in a graveyard

            // When Animate Dead enters the battlefield, if it’s on the battlefield, it loses “enchant creature card in a graveyard” and
            // gains “enchant creature put onto the battlefield with Animate Dead.” Return enchanted creature card to the battlefield 
            // under your control and attach Animate Dead to it. When Animate Dead leaves the battlefield, that creature’s controller 
            // sacrifices it.

            // Enchanted creature gets - 1 / -0.

            card.OnCast = game =>
            {
                var target = card.Controller.ChooseTarget(card, new List<ITarget>(card.Controller.Graveyard.Where(_c => _c.IsACreature))) as Card;
                card.SetVar("Target", target);
                card.AddEffect(new AnimateDeadEffect(card, target));
            };

            card.OnResolve = game =>
            {
                var target = card.GetVar<Card>("Target");
                // TODO: Move creature card from graveyard to battlefield
            };

            return card;
        }
    }

    public class AnimateDeadEffect : ContinuousEffect
    {
        private Card target;
        private IntModifier modifier;

        public AnimateDeadEffect(IResolvable source, Card target) : base(source)
        {
            this.target = target;
            modifier = new IntModifier(source, nameof(Card.Power), -1);
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == target)
            {
                (resolvable as Card).Modifiers.Add(modifier);
            }
        }

        public override void UnmodifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable is Card)
            {
                var card = resolvable as Card;
                if (card.Modifiers.Contains(modifier))
                    card.Modifiers.Remove(modifier);
            }
        }
    }
}
