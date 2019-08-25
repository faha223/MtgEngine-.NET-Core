using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Alpha.Enchantments
{
    [MtgCard("Burrowing", "LEA", "", "", "Enchant Creature\nEnchanted creature has mountainwalk.")]
    public class Burrowing : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
            card.Cost = ManaCost.Parse(card, "{R}");

            card.OnCast = (g, c) =>
            {
                var target = c.Controller.ChooseTarget(c, new List<ITarget>(g.Battlefield.Where(_c => _c.IsAnArtifact && _c.CanBeTargetedBy(c)))) as Card;
                c.AddEffect(new BurrowingEffect(c, target));
            };

            return card;
        }
    }

    public class BurrowingEffect : Effect
    {
        private Card target;
        private Modifier modifier;

        public BurrowingEffect(Card source, Card target) : base(source)
        {
            this.target = target;
            modifier = new StaticAbilityModifier(source, nameof(Card.StaticAbilities), ModifierMode.Add, StaticAbility.Mountainwalk);
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable is Card)
            {
                var card = resolvable as Card;
                if(!card.Modifiers.Contains(modifier))
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
