﻿using MtgEngine.Common.Enums;
using System.Collections.Generic;

namespace MtgEngine.Common.Mana
{
    /// <summary>
    /// A simple mana pool class that can track the amount of mana a player has access to at the moment.
    /// This, in its current state, will not be sufficient for cards like [Rosheen Meanderer] that put stipulations on how the mana they generate can be spent.
    /// </summary>
    public class ManaPool : Dictionary<ManaColor, int>
    {
        public ManaPool()
        {
            this[ManaColor.White] = 0;
            this[ManaColor.Blue] = 0;
            this[ManaColor.Black] = 0;
            this[ManaColor.Red] = 0;
            this[ManaColor.Green] = 0;
            this[ManaColor.Colorless] = 0;
        }
    }
}
