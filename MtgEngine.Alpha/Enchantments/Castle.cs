using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Effects;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtgEngine.Alpha.Enchantments
{
    [MtgCard("Castle", "LEA", "", "", "Untapped creatures you control get +0/+2")]
    public class Castle : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}{W}");
            card.AddEffect(new CastleEffect(card));

            return card;
        }
    }

    public class CastleEffect : ContinuousEffect
    {
        Modifier modifier;

        public CastleEffect(IResolvable source) : base(source)
        {
            modifier = new IntModifier(source, nameof(Card.Toughness), 2);
        }

        public override void ModifyObject(Game game, IResolvable resolvable)
        {
            if(resolvable is Card)
            {
                var card = resolvable as Card;
                if (card.Controller == Source.Controller && !card.IsTapped)
                {
                    if(!card.Modifiers.Contains(modifier))
                        card.Modifiers.Add(modifier);
                }
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
