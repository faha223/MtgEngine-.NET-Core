using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Abilities
{
    public abstract class Ability : IResolvable
    {
        public PermanentCard Source { get; private set; }

        public string Text { get; private set; }

        protected Ability(PermanentCard source, string text)
        {
            Source = source;
            Text = text;
        }

        public abstract void OnResolve(Game game);

        public abstract Ability Copy(PermanentCard newSource);
    }
}
