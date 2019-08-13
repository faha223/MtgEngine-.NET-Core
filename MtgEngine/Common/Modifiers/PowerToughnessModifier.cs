using System;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class PowerToughnessModifier : Modifier
    {
        public Func<Game, Card, int> Value { get; set; }

        public PowerToughnessModifier(IResolvable source, string property, Func<Game, Card, int> value) : base(source, property, ModifierMode.Override)
        {
            Value = value;
        }
    }
}
