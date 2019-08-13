using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;
using System;
using System.Linq;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card : IDamageable
    {
        public Func<Game, Card, int> BasePowerFunc = (game, card) => { return 0; };

        public int Power
        {
            get
            {
                var basePower = BasePowerFunc(Controller.Game, this);
                if(Modifiers.Any(c => c.Property == nameof(BasePowerFunc)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(BasePowerFunc)) as PowerToughnessModifier;
                    basePower = modifier.Value(Controller.Game, this);
                }

                return basePower
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    + (2 * Counters.Count(c => c == CounterType.Plus2Plus0))
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        public Func<Game, Card, int> BaseToughnessFunc = (game, card) => { return 0; };

        public int Toughness
        {
            get
            {
                var baseToughness = BaseToughnessFunc(Controller.Game, this);
                if(Modifiers.Any(c => c.Property == nameof(BaseToughnessFunc)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(BaseToughnessFunc)) as PowerToughnessModifier;
                    baseToughness = modifier.Value(Controller.Game, this);
                }

                return baseToughness
                    + Counters.Count(c => c == CounterType.Plus1Plus1)
                    + (2 * Counters.Count(c => c == CounterType.Plus0Plus2))
                    - Counters.Count(c => c == CounterType.Minus1Minus1);
            }
        }

        public bool HasSummoningSickness { get; set; } = false;
    }
}
