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
    // TODO: Make this an AuraCard that targets Enchantments
    [MtgCard("Animate Artifact", "LEA", "", "", "", "Enchant Artifact\nAs long as enchanted artifact isn't a creature, it’s an artifact creature with power and toughness each equal to its converted mana cost.")]
    public class AnimateArtifact : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}{U}");

            card.OnCast = game =>
            {
                var target = card.Controller.ChooseTarget(card, new List<ITarget>(game.Battlefield.Where(_c => _c.IsAnArtifact))) as Card;
                card.AddEffect(new AnimateArtifactEffect(card, target));
            };

            return card;
        }
    }

    public class AnimateArtifactEffect : ContinuousEffect
    {
        private Card target;
        private List<Modifier> modifiers = new List<Modifier>();

        public AnimateArtifactEffect(IResolvable source, Card target) : base(source)
        {
            this.target = target;

            modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Add, CardType.Creature));
            modifiers.Add(new PowerToughnessModifier(this, nameof(Card.BasePowerFunc), (game, card) =>
            {
                if (card.Cost is ManaCost)
                {
                    return (card.Cost as ManaCost).ConvertedManaCost;
                }
                return 0;
            }));
            modifiers.Add(new PowerToughnessModifier(this, nameof(Card.BaseToughnessFunc), (game, card) =>
            {
                if (card.Cost is ManaCost)
                {
                    return (card.Cost as ManaCost).ConvertedManaCost;
                }
                return 0;
            }));
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == target)
            {
                var card = resolvable as Card;
                if(!card.IsACreature)
                {
                    modifiers.ForEach(modifier => { if (card.Modifiers.Contains(modifier)) card.Modifiers.Add(modifier); });
                }
            }
        }

        public override void UnmodifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable is Card)
            {
                var card = resolvable as Card;
                foreach(var modifier in modifiers)
                {
                    if (card.Modifiers.Contains(modifier))
                        card.Modifiers.Remove(modifier);
                }
            }
        }
    }
}
