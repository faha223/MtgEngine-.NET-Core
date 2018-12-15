using MtgEngine;
using MtgEngine.Common.Players;
using MtgEngine.Common.Players.AIPlayers;

namespace MtgEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.AddPlayer(new ConsolePlayer("Specialfred453", 20, "60x Forest"));
            game.AddPlayer(new PassPriorityPlayer("Al", 20, "60x Forest"));
            game.Start().Wait();
        }
    }
}
