using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
                    .Where(x => x.IsSubclassOf(typeof(Card)));
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
                if (name == getCardName(cardType))
                    return cardType;
            }
            return null;
        }

        private static string getCardName(Type cardType)
        {
            var mtgCardAttribute = cardType.GetCustomAttributes(typeof(MtgCardAttribute), true).FirstOrDefault() as MtgCardAttribute;
            if (mtgCardAttribute != null)
                return mtgCardAttribute.Name;
            return null;
        }
    }
}
