﻿using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Abilities
{
    public abstract class Ability : IResolvable
    {
        public Card Source { get; private set; }

        public string Text { get; private set; }

        protected Ability(Card source, string text)
        {
            Source = source;
            Text = text;
        }

        public abstract void OnResolve(Game game);

        public abstract Ability Copy(Card newSource);
    }
}
