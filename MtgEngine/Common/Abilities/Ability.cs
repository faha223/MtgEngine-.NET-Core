namespace MtgEngine.Common
{
    public abstract class Ability : IResolvable
    {
        public Player Controller { get; private set; }

        protected Ability(Player controller)
        {
            Controller = controller;
        }

        public abstract void OnResolve(Game game);
    }
}
