using MtgEngine.Common.Players;
using MtgEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MtgEngine.Common.Cards
{
    public class Deck : List<Card>
    {
        private static Regex deckListEntryMatchRegex = new Regex(@"(\d+)x\s+(.*)");

        public Deck(Player owner, string deckList)
        {
            foreach(var entry in deckList.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()))
            {
                var matches = deckListEntryMatchRegex.Match(entry);
                int quantity = int.Parse(matches.Groups[1].Value);
                string cardName = matches.Groups[2].Value;
                var type = AllCards.GetCard(cardName);
                var ctor = type.GetCardCtor();

                for (int i = 0;i < quantity; i++)
                {
                    Add(ctor(owner));
                }
            }
        }
    }
}
