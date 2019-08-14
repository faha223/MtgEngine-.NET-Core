using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtgEngine.TestSet.Enchantments
{
    [MtgCard("Imprisoned in the Moon", "TestSet", "", "", "Enchant creature, land, or planeswalker\nEnchanted permanent is a colorless land with \"{ T}: Add {C}\" and loses all other card types and abilities.", "Only one vault was great enough to hold Emrakul.")]
    public class ImprisonedInTheMoon : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Abilities.Add(new ImprisonedInTheMoonAbility(card));

            card.OnCast = (game) =>
            {
                var target = card.Controller.ChooseTarget(card, new List<ITarget>(game.Battlefield.Where(c => (c.IsACreature || c.IsALand || c.IsAPlaneswalker) && c.CanBeTargetedBy(card))));
                card.SetVar("Target", target);
            };

            return card;
        }
    }

    public class ImprisonedInTheMoonAbility : EnchantPermanentAbility
    {
        public ImprisonedInTheMoonAbility(Card source) : base(source, "Enchant creature, land, or planeswalker\nEnchanted permanent is a colorless land with \"{ T}: Add {C}\" and loses all other card types and abilities.")
        {
        }

        public override Ability Copy(Card newSource)
        {
            return new ImprisonedInTheMoonAbility(newSource);
        }

        public override void Enchant(Card target)
        {
            // Enchanted permanent is colorless
            target.Modifiers.Add(new ColorModifier(this, nameof(Card.ColorIdentity), ModifierMode.Override, ManaColor.Colorless));

            // Enchanted permanent is a land and loses all other types
            target.Modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Override, CardType.Land));
            target.Modifiers.Add(new StringModifier(this, nameof(Card.Subtypes), ModifierMode.Override, null));

            // With "{T}: Add {C}" and loses all other abilities
            target.Modifiers.Add(new AbilityModifier(this, nameof(Card.Abilities), ModifierMode.Override, new ManaAbility(target, new TapCost(target), new Common.Mana.ManaAmount(1, ManaColor.Colorless), "{T}: Add {C}.")));
            target.Modifiers.Add(new StaticAbilityModifier(this, nameof(Card.StaticAbilities), ModifierMode.Override, null));
        }

        public override void Disenchant(Card target)
        {
            target.Modifiers.RemoveAll(c => c.Source == this);
        }

    }
}
