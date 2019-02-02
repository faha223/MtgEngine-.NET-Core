using System;

namespace MtgEngine.Common.Exceptions
{
    public class ActivePlayerLostTheGameException : Exception
    {
        public ActivePlayerLostTheGameException(string reason) : base(reason)
        {
        }
    }
}
