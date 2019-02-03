using MtgEngine.Common.Players;
using System;

namespace MtgEngine.Common.Exceptions
{
    public class PlayerWonTheGameException : Exception
    {
        public Player Winner { get; private set; }

        public PlayerWonTheGameException(Player winner, string reason) : base(reason)
        {
            Winner = winner;
        }
    }
}
