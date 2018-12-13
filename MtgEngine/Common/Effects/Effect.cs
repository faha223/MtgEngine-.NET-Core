using System;

namespace MtgEngine.Common
{
    public abstract class Effect : IResolvable
    {
        public virtual void OnResolve(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
