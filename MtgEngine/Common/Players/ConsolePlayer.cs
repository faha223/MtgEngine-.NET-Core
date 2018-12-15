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
            Console.WriteLine($"You Have Priority (Active Player: {activePlayer.Name}");
            return new PassPriorityAction();
        }

        public override Card SelectTarget(string message, Func<Card, bool> targetSelector)
        {
            // TODO: Print a list of possible targets to the console and allow the user to select one, or none;
            return null;
        }
    }
}
