using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
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
            card.Abilities.Add(new DarksteelMutationAbility(card));

            card.OnCast = (game) =>
            {
                var target = card.Controller.ChooseTarget(card, new List<ITarget>(game.Battlefield.Creatures.Where(c => c.CanBeTargetedBy(card))));
                card.SetVar("Target", target);
            };

            return card;
        }
    }

    public class DarksteelMutationAbility : EnchantPermanentAbility
    {
        public DarksteelMutationAbility(Card source) : base(source, "Enchant Creature\nnchanted creature is an Insect artifact creature with base power and toughness 0/1 and has indestructible, and it loses all other abilities, card types, and creature types.")
        {
        }

        public override Ability Copy(Card newSource)
        {
            return new DarksteelMutationAbility(newSource);
        }

        public override void Enchant(Card target)
        {
            // Enchanted Creature is an Insect Artifact Creature
            target.Modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Override, CardType.Artifact));
            target.Modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Add, CardType.Creature));
            target.Modifiers.Add(new StringModifier(this, nameof(Card.Subtypes), "Insect"));

            // With base power and toughness 0/1
            target.Modifiers.Add(new PowerToughnessModifier(this, nameof(Card.BasePowerFunc), (game, card) => { return 0; }));
            target.Modifiers.Add(new PowerToughnessModifier(this, nameof(Card.BaseToughnessFunc), (game, card) => { return 1; }));

            // With indestructible and loses all other abilities
            target.Modifiers.Add(new AbilityModifier(this, nameof(Card.Abilities), ModifierMode.Override, null));
            target.Modifiers.Add(new StaticAbilityModifier(this, nameof(Card.StaticAbilities), ModifierMode.Override, null));
            target.Modifiers.Add(new StaticAbilityModifier(this, nameof(Card.StaticAbilities), ModifierMode.Add, StaticAbility.Indestructible));
        }

        public override void Disenchant(Card target)
        {
            target.Modifiers.RemoveAll(c => c.Source == this);
        }
    }
}
