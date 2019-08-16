using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Effects;
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

            card.Cost = ManaCost.Parse(card, "{2}{U}");

            card.OnCast = (g, c) =>
            {
                var target = c.Controller.ChooseTarget(c, new List<ITarget>(g.Battlefield.Where(_c => (_c.IsACreature || _c.IsALand || _c.IsAPlaneswalker) && _c.CanBeTargetedBy(c)))) as Card;
                c.AddEffect(new ImprisonedInTheMoonEffect(c, target));
            };

            return card;
        }
    }

    public class ImprisonedInTheMoonEffect : ContinuousEffect
    {
        private Card Target;
        private List<Modifier> modifiers = new List<Modifier>();

        public ImprisonedInTheMoonEffect(IResolvable source, Card target) : base(source)
        {
            Target = target;

            // Enchanted permanent is colorless
            modifiers.Add(new ColorModifier(this, nameof(Card.ColorIdentity), ModifierMode.Override, ManaColor.Colorless));

            // Enchanted permanent is a land and loses all other types
            modifiers.Add(new CardTypeModifier(this, nameof(Card.Types), ModifierMode.Override, CardType.Land));
            modifiers.Add(new StringModifier(this, nameof(Card.Subtypes), ModifierMode.Override, null));

            // With "{T}: Add {C}" and loses all other abilities
            modifiers.Add(new AbilityModifier(this, nameof(Card.Abilities), ModifierMode.Override, new ManaAbility(target, new TapCost(target), new Common.Mana.ManaAmount(1, ManaColor.Colorless), "{T}: Add {C}.")));
            modifiers.Add(new StaticAbilityModifier(this, nameof(Card.StaticAbilities), ModifierMode.Override, null));
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == Target)
            {
                modifiers.ForEach(modifier =>
                {
                    if(!Target.Modifiers.Contains(modifier))
                        Target.Modifiers.Add(modifier);
                });
            }
        }

        public override void UnmodifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable == Target)
            {
                modifiers.ForEach(modifier =>
                {
                    if(Target.Modifiers.Contains(modifier))
                        Target.Modifiers.Remove(modifier);
                });
            }
        }
    }
}
