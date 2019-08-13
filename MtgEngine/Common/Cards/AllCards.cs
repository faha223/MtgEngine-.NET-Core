using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MtgEngine.Common.Cards
{
    public static class AllCards
    {
        private static List<Type> Cards = new List<Type>();
        static AllCards()
        {
            foreach(var dll in Directory.EnumerateFiles(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "MtgEngine.*.dll"))
            {
                var sources = Assembly.LoadFile(dll)
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(CardSource)));
                foreach(var type in sources)
                {
                    Cards.Add(type);
                }
            }
        }

        public static Type GetCard(string name)
        {
            foreach (var cardType in Cards)
            {
                if (name == MtgCardAttribute.GetAttribute(cardType)?.Name)
                    return cardType;
            }
            return null;
        }
    }
}
