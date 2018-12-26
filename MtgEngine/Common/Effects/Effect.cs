using System;

namespace MtgEngine.Common
{
    public abstract class Effect : IResolvable
    {
        public abstract void OnResolve(Game game);
    }
}
