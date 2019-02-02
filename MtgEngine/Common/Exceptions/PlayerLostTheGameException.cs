using System;
using System.Collections.Generic;
using System.Text;

namespace MtgEngine.Common.Exceptions
{
    public class PlayerLostTheGameException : Exception
    {
        public PlayerLostTheGameException(string reason) : base(reason)
        {
        }
    }
}
