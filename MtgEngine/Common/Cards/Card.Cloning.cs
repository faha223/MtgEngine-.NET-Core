using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : ITarget, IResolvable
    {
        public IResolvable copyingSource { get; private set; }
		public Card IsCopying { get; private set; }

		public void Copy(Card target, IResolvable source)
        {
            // Can't copy itself
            if (target == this)
                return;

			while(target.IsCopying != null)
            {
                target = target.IsCopying;
            }

            IsCopying = target;
            copyingSource = source;

            Modifiers.Add(new StringModifier(source, nameof(Name), target.Name));
            Modifiers.Add(new StringModifier(source, nameof(Text), target.Text));
            Modifiers.Add(new StringModifier(source, nameof(FlavorText), target.FlavorText));
            Modifiers.Add(new BooleanModifier(source, nameof(IsLegendary), target.IsLegendary));
            Modifiers.Add(new BooleanModifier(source, nameof(IsSnow), target.IsSnow));
            Modifiers.Add(new CostModifier(source, nameof(Cost), target.Cost.Copy(this)));

            Modifiers.Add(new PowerToughnessModifier(source, nameof(BasePowerFunc), target.BasePowerFunc));
            Modifiers.Add(new PowerToughnessModifier(source, nameof(BaseToughnessFunc), target.BaseToughnessFunc));

            Modifiers.Add(new CardTypeModifier(source, nameof(Types), ModifierMode.Override, null));
            foreach (var type in target.Types)
            {
                Modifiers.Add(new CardTypeModifier(source, nameof(Types), ModifierMode.Add, type));
            }

            Modifiers.Add(new StringModifier(source, nameof(Subtypes), null));
            if (target.Subtypes != null)
            {
                foreach (var subtype in target.Subtypes)
                {
                    Modifiers.Add(new StringModifier(source, nameof(Subtypes), ModifierMode.Add, subtype));
                }
            }

            Modifiers.Add(new StaticAbilityModifier(source, nameof(StaticAbilities), ModifierMode.Override, null));
            foreach (var staticAbility in target.StaticAbilities)
            {
                Modifiers.Add(new StaticAbilityModifier(source, nameof(StaticAbilities), ModifierMode.Add, staticAbility));
            }

            Modifiers.Add(new AbilityModifier(source, nameof(Abilities), ModifierMode.Override, null));
            foreach (var ability in target.Abilities)
            {
                Modifiers.Add(new AbilityModifier(this, nameof(Abilities), ModifierMode.Add, ability.Copy(this)));
            }
        }

        public void StopCopying(Card target, IResolvable source)
        {
            if (IsCopying == target)
                IsCopying = null;
            Modifiers.RemoveAll(c => c.Source == source);
        }
    }
}
