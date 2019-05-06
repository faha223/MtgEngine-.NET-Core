using MtgEngine.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MtgEngine.Common.Mana
{
    public static class ManaParser
    {
        public static ManaAmount[] Parse(string manaCost)
        {
            // If the mana cost is 0, then there isn't really a mana cost to be paid


            if (manaCost == "{0}")
                return null;

            // TODO: Add Logic to parse Mana Costs, 
            // Suggested approach: maybe use a regex to extract matches from the string, and then
            // convert them to ManaAmounts in a list, and then return the list as an array

            var regex = new Regex(@"(\{[0-9XWUBRGCP\/]+\})");
            var genericRegex = new Regex(@"\{([0-9]+)\}");
            var matches = regex.Matches(manaCost);

            if (matches.Count == 0)
                return null;

            List<ManaAmount> amounts = new List<ManaAmount>(matches.Count);

            foreach (Match match in matches)
            {
                var manaColor = FromString(match.Value);
                if (manaColor.HasValue)
                {
                    if (amounts.Any(c => c.Color == manaColor.Value))
                    {
                        var amt = amounts.First(c => c.Color == manaColor.Value);
                        amounts.Remove(amt);
                        amounts.Add(new ManaAmount(amt.Amount + 1, amt.Color));
                    }
                    else
                        amounts.Add(new ManaAmount(1, manaColor.Value));
                }
                else if (genericRegex.IsMatch(match.Value))
                {
                    amounts.Add(new ManaAmount(int.Parse(match.Value.Substring(1, match.Value.Length - 2)), ManaColor.Generic));
                }
                else
                    return null;
            }

            return amounts.ToArray();
        }

        private static ManaColor? FromString(string str)
        {
            foreach (var color in (IEnumerable<ManaColor>)Enum.GetValues(typeof(ManaColor)))
            {
                var attrib = ManaAttribute.GetManaAttribute(color);
                if (attrib != null)
                {
                    if (attrib.AsString == str)
                        return color;
                }
            }
            return null;
        }
    }
}
