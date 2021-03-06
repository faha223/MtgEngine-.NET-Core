﻿using MtgEngine.Common.Counters;

namespace MtgEngine.Common.Enums
{
    public enum CounterType
    {
        [Counter("Charge")]
        Charge,

        [Counter("+1/+1")]
        Plus1Plus1,

        [Counter("+2/+0")]
        Plus2Plus0,

        [Counter("+0/+2")]
        Plus0Plus2,

        [Counter("-1/-1")]
        Minus1Minus1,

        [Counter("Loyalty")]
        Loyalty,

        [Counter("Spore")]
        Spore,

        [Counter("Energy")]
        Energy,

        [Counter("Ice")]
        Ice,

        [Counter("Corpse")]
        Corpse,

        [Counter("Poison")]
        Poison
    }
}
