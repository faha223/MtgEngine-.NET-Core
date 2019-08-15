using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Effects;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.TestSet.Enchantments
{
    [MtgCard("Darksteel Mutation", "TestSet", "", "", "Enchant Creature\nnchanted creature is an Insect artifact creature with base power and toughness 0/1 and has indestructible, and it loses all other abilities, card types, and creature types.", "Infinitely powerless.")]
    public class DarksteelMutation : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}{W}");
            
            card.OnCast = game =>
            {
                var target = card.Controller.ChooseTarget(card, new List<ITarget>(game.Battlefield.Creatures.Where(c => c.CanBeTargetedBy(card)))) as Card;
                card.AddEffect(new DarksteelMutationEffect(card, target));
            };

            return card;
        }
    }

    public class DarksteelMutationEffect : ContinuousEffect
    {
        private Card target;
        private List<Modifier> modifiers = new List<Modifier>();

        public DarksteelMutationEffect(IResolvable source, Card target) : base(source)
        {
            this.target = target;

            // Enchanted Creature is an Insect Artifact Creature
            modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Override, CardType.Artifact));
            modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Add, CardType.Creature));
            modifiers.Add(new StringModifier(this, nameof(Card.Subtypes), "Insect"));

            // With base power and toughness 0/1
            modifiers.Add(new PowerToughnessModifier(this, nameof(Card.BasePowerFunc), (game, card) => { return 0; }));
            modifiers.Add(new PowerToughnessModifier(this, nameof(Card.BaseToughnessFunc), (game, card) => { return 1; }));

            // With indestructible and loses all other abilities
            modifiers.Add(new AbilityModifier(this, nameof(Card.Abilities), ModifierMode.Override, null));
            modifiers.Add(new StaticAbilityModifier(this, nameof(Card.StaticAbilities), ModifierMode.Override, null));
            modifiers.Add(new StaticAbilityModifier(this, nameof(Card.StaticAbilities), ModifierMode.Add, StaticAbility.Indestructible));
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == target)
            {
                modifiers.ForEach(modifier => target.Modifiers.Add(modifier));
            }
        }

        public override void UnmodifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == target)
            {
                modifiers.ForEach(modifier => target.Modifiers.Remove(modifier));
            }
        }
    }
}
