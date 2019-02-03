using MtgEngine;
using MtgEngine.Common.Players.AIPlayers;
using System.IO;
using System.Reflection;

namespace MtgEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var deckFileLocation = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "PlayerDeck.txt");

            var game = new Game();
            game.AddPlayer(new ConsolePlayer("Specialfred453", 20, File.ReadAllText(deckFileLocation)));
            //game.AddPlayer(new ConsolePlayer("Not Fred", 20, File.ReadAllText(deckFileLocation)));
            game.AddPlayer(new PassPriorityPlayer("Al", 20, File.ReadAllText(deckFileLocation)));
            game.Start().Wait();
            System.Console.Write("Game Over. Press Enter to Exit...");
            System.Console.ReadLine();
        }
    }
}
