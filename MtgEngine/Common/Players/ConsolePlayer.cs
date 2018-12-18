using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Players.Actions;
using MtgEngine.Common.Players.Gameplay;
using MtgEngine.Common.Utilities;

namespace MtgEngine.Common.Players
{
    public class ConsolePlayer : Player
    {
        public ConsolePlayer(string name, int startingLifeTotal, string deckList) : base(name, startingLifeTotal, deckList)
        {
        }

        public override ActionBase GivePriority(Player activePlayer, bool canPlaySorcerySpeedSpells)
        {
            // TODO: Print to the Console a list of possible actions, and allow the user to select one
            PrintHand();
            Console.WriteLine($"You Have Priority (Active Player: {activePlayer.Name})");
            return new PassPriorityAction();
        }

        private void PrintHand()
        {
            Console.WriteLine("Your Hand:");
            Hand.ForEach(card => Console.WriteLine(card.Name));
        }

        public override void Draw(int howMany)
        {
            var cardsDrawn = Library.Take(howMany).ToList();
            Library.RemoveRange(0, howMany);
            cardsDrawn.ForEach(card =>
            {
                Console.WriteLine($"You Drew: {card.Name}");
                Hand.Add(card);
            });
        }

        public override void Discard()
        {
            Console.WriteLine("Your Hand:");
            for(int i = 0; i < Hand.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {Hand[i].Name}");
            }

            Console.Write("Choose a card to discard: ");
            string choice = Console.ReadLine();
            int index = -1;
            if(int.TryParse(choice, out index))
            {
                if(index > 0 && index <= Hand.Count)
                {
                    Discard(Hand[index - 1]);
                }
            }
        }

        public override bool OfferMulligan()
        {
            // We can't mulligan anymore
            if (Hand.Count == 0)
                return false;

            PrintHand();
            
            while (true)
            {
                Console.WriteLine();
                Console.Write("Do you wish to take a mulligan? (Y/N)");
                char choice = Convert.ToChar(Console.Read());
                if (choice == 'y')
                    return true;
                else if (choice == 'n')
                    return false;
            }
        }

        public override Card SelectTarget(string message, Func<Card, bool> targetSelector)
        {
            // TODO: Print a list of possible targets to the console and allow the user to select one, or none;
            return null;
        }

        public override void ScryChoice(List<Card> scryedCards, out IEnumerable<Card> cardsOnTop, out IEnumerable<Card> cardsOnBottom)
        {
            Console.WriteLine($"Scry {scryedCards.Count}");
            for(int i = 0; i < scryedCards.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {scryedCards[i].Name}");
            }

            List<int> choice = null;
            do
            {
                Console.Write("Which cards would you like to put on the bottom of your library (in the order you wish to draw them)." + Environment.NewLine
                    + "Don't type anything and just press enter if you wish to keep all on top: ");
                choice = ParseChoice(Console.ReadLine(), 0, scryedCards.Count, 1, scryedCards.Count, true);

                // If we have to repeat this, then lets put a blank line since the last request to make it easier to see
                if (choice == null)
                    Console.WriteLine();
            } while (choice == null);

            cardsOnBottom = scryedCards.OrderByIndexList(choice.Select(c => c - 1));
            
            scryedCards = scryedCards.Except(cardsOnBottom).ToList();
            if (scryedCards.Count > 1)
            {
                Console.WriteLine("Cards to be put on top:");
                for (int i = 0; i < scryedCards.Count; i++)
                    Console.WriteLine($"{i + 1}: {scryedCards[i].Name}");

                choice = null;
                do
                {
                    Console.Write("Choose the order the remaining cards should be placed on top of the library. First card is the next card you draw.");
                    choice = ParseChoice(Console.ReadLine(), scryedCards.Count, scryedCards.Count, 1, scryedCards.Count, true);

                    // If we have to repeat this, then lets put a blank line since the last request to make it easier to see
                    if (choice == null)
                        Console.WriteLine();
                } while (choice == null);

                cardsOnTop = scryedCards.OrderByIndexList(choice.Select(c => c-1));
            }
            else
                cardsOnTop = scryedCards;
        }

        public override List<AttackerDeclaration> DeclareAttackers()
        {
            Console.WriteLine("TODO: Declare Attackers");
            return null;
        }

        public override List<BlockerDeclaration> DeclareBlockers(IEnumerable<CreatureCard> attackingCreatures)
        {
            // TODO: Let the player chose which of their creatures will block which of the attacker's creatures
            Console.WriteLine("TODO: Declare Blockers");
            return null;
        }

        private List<int> ParseChoice(string userText, int minResponseCount, int maxResponseCount, int minResponseValue, int maxResponseValue, bool noDuplicates)
        {
            if (string.IsNullOrWhiteSpace(userText))
            {
                if (minResponseCount <= 0)
                    return new List<int>();
                else
                    Console.WriteLine($"You must choose at least {minResponseCount}");
                    return null;
            }
            // This regex expects a list of integers, separated by a minimum of one comma or space
            userText = Regex.Replace(userText, @"[,\s]\s*", ","); // Clean up the commas
            userText = Regex.Replace(userText, ",+", ",");              // Each selection split by only 1 comma
            userText = Regex.Replace(userText, @"^,", string.Empty);    // No leading commas    
            userText = Regex.Replace(userText, @",$", string.Empty);    // No trailing commas

            // Check that it matches the regex we have rigorously tried to shoehorn the user text into
            if(Regex.IsMatch(userText, @"\d+(,\d+)*"))
            {
                try
                {
                    // Split the list and parse each integer. This will throw an exception if an integer doesn't parse
                    List<int> selection = userText.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToList();

                    // Check that the user selected at least the minimum number of items
                    if (selection.Count > maxResponseCount)
                    {
                        Console.WriteLine($"You may only choose {maxResponseCount} items");
                        return null;
                    }
                    else if (selection.Count < minResponseCount)
                    {
                        Console.WriteLine($"You must choose at least {minResponseCount} items");
                        return null;
                    }

                    // Check the validity of each response
                    foreach(var item in selection)
                    {
                        if (item < minResponseValue || item > maxResponseValue)
                        {
                            Console.WriteLine($"Invalid Response: {item}");
                            return null;
                        }
                        else if(noDuplicates && selection.Count(c => c == item) > 1)
                        {
                            Console.WriteLine($"You chose {item} more than once");
                            return null;
                        }
                    }

                    return selection;
                }
                catch
                {
                    Console.WriteLine("I don't understand your response.");
                }
            }
            else
            {
                Console.WriteLine("I don't understand your response.");
            }

            return null;
        }
    }
}
