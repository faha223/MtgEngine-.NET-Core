using MtgEngine.Common.Cards;
using System;
using System.Linq;

namespace MtgEngineTest.Helpers
{
    public static class CardExtensions
    {
        public static void PrintCard(this Card card)
        {
            // Skip a line before printing
            Console.WriteLine();

            // Print the Name and CMC
            card.PrintNameCMCLine();

            // Print the Type Line
            if (card.IsLegendary)
                Console.Write("Legendary ");
            if (card.IsBasic)
                Console.Write("Basic ");
            Console.Write($"{string.Join(" ", card.Types)}");

            if (card.Subtypes != null && card.Subtypes.Count() > 0)
                Console.WriteLine($" - {string.Join(" ", card.Subtypes)}");
            else
                Console.WriteLine();
            printSeparator();

            // Print the Abilities
            if (card.Abilities != null && card.Abilities.Count > 0)
            {
                int abilitiesPrinted = 0;
                foreach (var ability in card.Abilities)
                {
                    if (abilitiesPrinted > 0)
                        Console.WriteLine();
                    Console.WriteLine(ability.Text.Wrap(40));
                    abilitiesPrinted++;
                }
            }
            Console.WriteLine();
            
            // Print the Flavor Text
            if (!string.IsNullOrWhiteSpace(card.PrintedFlavorText))
            {
                Console.WriteLine("             +-------------+             ");
                Console.WriteLine(card.PrintedFlavorText.Wrap(40));
            }
            printSeparator();

            // Print the Power and Toughness
            if (card.Types.Any(c => c == MtgEngine.Common.Enums.CardType.Creature) || card.Subtypes.Any(c => c == "Vehicle"))
                Console.WriteLine($"{card.Power}/{card.Toughness}".PadLeft(40));
            
            // Skip a line before printing again
            Console.WriteLine();
        }

        private static void PrintNameCMCLine(this Card card)
        {
            if(card.Types.Any(c => c == MtgEngine.Common.Enums.CardType.Land))
            {
                Console.WriteLine(card.Name.Substring(0, Math.Min(card.Name.Length, 40)));
            }
            else
            {
                string name = card.Name;
                string cmc = card.Cost.ToString();
                int nameFieldLength = 40 - cmc.Length;

                if (name.Length > nameFieldLength)
                    name = name.Substring(0, nameFieldLength);

                Console.Write($"{name.PadRight(nameFieldLength)}");
                Console.WriteLine($"{cmc}");
            }
            
            printSeparator();
        }

        private static void printSeparator(int length = 40)
        {
            for (int i = 0; i < length; i++)
                Console.Write("-");
            Console.WriteLine();
        }
    }
}
