﻿using MtgEngine.Common.Counters;

namespace MtgEngine.Common.Enums
{
    public enum CounterType
    {
        [Counter("Charge")]
        Charge,

        [Counter("+1/+1")]
        Plus1Plus1,

        [Counter("-1/-1")]
        Minus1Minus1,

        [Counter("Loyalty")]
        Loyalty
    }
}