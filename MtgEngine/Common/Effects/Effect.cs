using MtgEngine.Common.Players;

namespace MtgEngine.Common
{
    public abstract class Effect : IResolvable
    {
        public Player Controller
        {
            get
            {
                return Source.Controller;
            }
        }

        public IResolvable Source { get; set; }

        public Effect(IResolvable source)
        {
            Source = source;
        }

        public abstract void ModifyObject(Game game, IResolvable resolvable);

        public abstract void UnmodifyObject(Game game, IResolvable resolvable);
    }
}
