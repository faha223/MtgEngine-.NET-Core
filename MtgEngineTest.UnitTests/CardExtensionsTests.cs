using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtgEngineTest.Helpers;
using System.Linq;

namespace MtgEngineTest.UnitTests
{
    [TestClass]
    public class CardExtensionsTests
    {
        private void printCardTest(string cardName)
        {
            var decklist = $"1x {cardName}";
            var player = new ConsolePlayer("TestPlayer", 20, decklist);
            player.Draw(1);
            player.Hand.First().PrintCard();
        }

        [TestMethod]
        public void TestPrintImprisonedInTheMoon()
        {
            printCardTest("Imprisoned in the Moon");
        }
    }
}
