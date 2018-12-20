using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;
using System;
using System.Reflection;

namespace MtgEngine.Common.Utilities
{
    public static class ReflectionUtils
    {
        public delegate Card CardCtor(Player owner);
        
        public static CardCtor GetCardCtor(this Type type)
        {
            var ctor = type.GetConstructor(
              BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
              CallingConventions.Any,
              new[] { typeof(Player) }, null);

            if (ctor == null)
                return null;

            return (Player owner) =>
            {
                return (Card)ctor.Invoke(new[] { owner });
            };
        }
    }
}
