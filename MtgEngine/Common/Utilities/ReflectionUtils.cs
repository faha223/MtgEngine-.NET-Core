using MtgEngine.Common.Cards;
using System;
using System.Reflection;

namespace MtgEngine.Common.Utilities
{
    public static class ReflectionUtils
    {
        public delegate CardSource CardCtor();
        
        public static CardCtor GetCardCtor(this Type type)
        {
            var ctor = type.GetConstructor(
              BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
              CallingConventions.Any,
              new Type[0], null);

            if (ctor == null)
                return null;

            return () =>
            {
                return (CardSource)ctor.Invoke(null);
            };
        }
    }
}
