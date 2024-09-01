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
    // TODO: Finish when ProtectionFromBlack has been implemented

    [MtgCard("Black Ward", "LEA", "", "", Text= "Enchant creature\n\nEnchanted creature has protection from black.This effect doesn’t remove Black Ward.")]
    public class BlackWard : CardSource
    {
        public Card enchantedCreature;

        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false, false, false);
            card._attrs = CardAttrs;

            card.Cost = ManaCost.Parse(card, "{W}");

            card.OnCast = (g, c) =>
            {
                var target = c.Controller.ChooseTarget(c, new List<ITarget>(g.Battlefield.Creatures.Where(_c => _c.CanBeTargetedBy(c)))) as Card;
                c.SetVar("Target", target);
                c.AddEffect(new BlackWardEffect(c, target));
            };

            return card;
        }
    }

    public class BlackWardEffect : ContinuousEffect
    {
        private Card target;
        private Modifier modifier;

        public BlackWardEffect(IResolvable source, Card target) : base(source)
        {
            this.target = target;

            modifier = new StaticAbilityModifier(source, nameof(Card.StaticAbilities), ModifierMode.Add, null);// StaticAbility.ProtectionFromBlack);
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == target)
            {
                var card = resolvable as Card;
                card.Modifiers.Add(modifier);
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
