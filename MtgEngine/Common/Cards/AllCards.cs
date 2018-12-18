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
                if (name == GetCardName(cardType))
                    return cardType;
            }
            return null;
        }

        private static MtgCardAttribute getMtgCardAttribute(Type cardType)
        {
            var attributes = cardType.GetCustomAttributes(typeof(MtgCardAttribute), true);
            return attributes.FirstOrDefault() as MtgCardAttribute;
        }

        public static string GetCardName(Type cardType)
        {
            return getMtgCardAttribute(cardType)?.Name;
        }

        public static string GetSet(Type cardType)
        {
            return getMtgCardAttribute(cardType)?.SetName;
        }

        public static string GetCardId(Type cardType)
        {
            return getMtgCardAttribute(cardType)?.CardId;
        }

        public static string GetImageUri(Type cardType)
        {
            return getMtgCardAttribute(cardType)?.ImageUri;
        }

        public static string GetText(Type cardType)
        {
            return getMtgCardAttribute(cardType)?.Text;
        }

        public static string GetFlavorText(Type cardType)
        {
            return getMtgCardAttribute(cardType)?.FlavorText;
        }
    }
}
