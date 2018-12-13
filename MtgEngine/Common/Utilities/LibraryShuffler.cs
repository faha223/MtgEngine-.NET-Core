using MtgEngine.Common.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtgEngine.Common.Utilities
{
    public static class LibraryShuffler
    {
        public static void ShuffleLibrary(List<Card> library)
        {
            List<Card> temp = new List<Card>(library.Count);
            temp.AddRange(library);
            library.Clear();
            Random rand = new Random();

            while (temp.Count > 0)
            {
                var card = temp[Math.Abs(rand.Next()) % temp.Count];
                temp.Remove(card);
                library.Add(card);
            }
        }
    }
}
