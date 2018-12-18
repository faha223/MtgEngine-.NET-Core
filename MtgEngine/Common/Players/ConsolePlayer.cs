using System;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Players.Actions;

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
            Console.WriteLine("Your Hand:");
            Hand.ForEach(card => Console.WriteLine(card.Name));
            Console.WriteLine($"You Have Priority (Active Player: {activePlayer.Name})");
            return new PassPriorityAction();
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

        public override Card SelectTarget(string message, Func<Card, bool> targetSelector)
        {
            // TODO: Print a list of possible targets to the console and allow the user to select one, or none;
            return null;
        }
    }
}
