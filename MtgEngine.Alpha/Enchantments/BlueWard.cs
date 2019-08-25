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
    // TODO: Finish when ProtectionFromBlue has been implemented

    [MtgCard("Blue Ward", "LEA", "", "", "Enchant Creature\nEnchanted creature has protection from blue. This effect doesn't remove blue ward.")]
    public class BlueWard : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{W}");

            card.OnCast = (g, c) =>
            {
                var target = c.Controller.ChooseTarget(c, new List<ITarget>(g.Battlefield.Creatures.Where(_c => _c.CanBeTargetedBy(c)))) as Card;
                c.SetVar("Target", target);
                c.AddEffect(new BlueWardEffect(c, target));
            };

            return card;
        }
    }

    public class BlueWardEffect : ContinuousEffect
    {
        private Card target;
        private Modifier modifier;

        public BlueWardEffect(IResolvable source, Card target) : base(source)
        {
            this.target = target;

            modifier = new StaticAbilityModifier(source, nameof(Card.StaticAbilities), ModifierMode.Add, null);// StaticAbility.ProtectionFromBlue);
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if (resolvable == target)
            {
                var card = resolvable as Card;
                card.Modifiers.Add(modifier);
            }
        }

        public override void UnmodifyObject(Game game, IResolvable resolvable)
        {
            if (resolvable is Card)
            {
                var card = resolvable as Card;
                if (card.Modifiers.Contains(modifier))
                    card.Modifiers.Remove(modifier);
            }
        }
    }
}
