namespace MtgEngine.Common
{
    public abstract class Ability : IResolvable
    {
        public Player Controller { get; private set; }

        public string Text { get; private set; }

        protected Ability(Player controller, string text)
        {
            Controller = controller;
            Text = text;
        }

        public abstract void OnResolve(Game game);
    }
}
