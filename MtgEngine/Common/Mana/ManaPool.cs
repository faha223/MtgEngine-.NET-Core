using MtgEngine.Common.Enums;
using System;
using System.Collections.Generic;

namespace MtgEngine.Common.Mana
{
    /// <summary>
    /// A simple mana pool class that can track the amount of mana a player has access to at the moment.
    /// This, in its current state, will not be sufficient for cards like [Rosheen Meanderer] that put stipulations on how the mana they generate can be spent.
    /// </summary>
    public class ManaPool
    {
        private readonly Dictionary<ManaColor, int> _pool = new Dictionary<ManaColor, int>();

        public ManaPool()
        {
            Clear();
        }

        public int this[ManaColor color]
        {
            get
            {
                switch (color)
                {
                    case ManaColor.White:
                    case ManaColor.Blue:
                    case ManaColor.Black:
                    case ManaColor.Red:
                    case ManaColor.Green:
                    case ManaColor.Colorless:
                        return _pool[color];
                    default:
                        throw new ArgumentException("Only White, Blue, Black, Red, Green, and Colorless mana can exist in the mana pool");
                }
            }
            set
            {
                switch (color)
                {
                    case ManaColor.White:
                    case ManaColor.Blue:
                    case ManaColor.Black:
                    case ManaColor.Red:
                    case ManaColor.Green:
                    case ManaColor.Colorless:
                        _pool[color] = value;
                        break;
                    default:
                        throw new ArgumentException("Only White, Blue, Black, Red, Green, and Colorless mana can exist in the mana pool");
                }
            }
        }

        public void Add(ManaAmount manaAmount)
        {
            if (manaAmount.Color == ManaColor.Generic)
                throw new ArgumentException("ManaAmount.Color: Invalid Value. Generic Mana can only be payed. Generic Mana cannot be generated.");

            _pool[manaAmount.Color] += manaAmount.Amount;
        }

        public ManaPool Clone()
        {
            var pool = new ManaPool();

            foreach (var color in new[] { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green, ManaColor.Colorless })
                pool[color] = this[color];

            return pool;
        }

        // Override the Clear behavior of the Dictionary 
        public void Clear()
        {
            _pool.Clear();
            _pool[ManaColor.White] = 0;
            _pool[ManaColor.Blue] = 0;
            _pool[ManaColor.Black] = 0;
            _pool[ManaColor.Red] = 0;
            _pool[ManaColor.Green] = 0;
            _pool[ManaColor.Colorless] = 0;
        }
    }
}
