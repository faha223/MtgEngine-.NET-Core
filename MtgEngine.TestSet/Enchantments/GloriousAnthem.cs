using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Effects;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Enchantments
{
    [MtgCard("Glorious Anthem", "TestSet", "", "", "Creatures you control get +1/+1", "Once heard, the battle song of an angel becomes part of the listener forever.")]
    public class GloriousAnthem : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}{W}{W}");

            card.AddEffect(new GloriousAnthemEffect(card));

            return card;
        }
    }

    public class GloriousAnthemEffect : ContinuousEffect
    {
        private IntModifier powerModifier;
        private IntModifier toughnessModifier;

        public GloriousAnthemEffect(IResolvable source) : base(source)
        {
            powerModifier = new IntModifier(this, nameof(Card.Power), 1);
            toughnessModifier = new IntModifier(this, nameof(Card.Toughness), 1);
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable != null && resolvable is Card)
            {
                var card = resolvable as Card;

                // This enchantment only applies power and toughness modifiers, and only creatures can meaningfully have power and toughness, so no need to check for that.
                if(card.IsACreature && card.Controller == Source.Controller)
                {
                    if(!card.Modifiers.Contains(powerModifier))
                        card.Modifiers.Add(powerModifier);
                    if(!card.Modifiers.Contains(toughnessModifier))
                        card.Modifiers.Add(toughnessModifier);
                }
            }
        }

        public override void UnmodifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable != null && resolvable is Card)
            {
                var card = resolvable as Card;

                if(card.Modifiers.Contains(powerModifier))
                    card.Modifiers.Remove(powerModifier);

                if(card.Modifiers.Contains(toughnessModifier))
                    card.Modifiers.Remove(toughnessModifier);
            }
        }
    }
}
