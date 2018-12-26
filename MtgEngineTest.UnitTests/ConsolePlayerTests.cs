using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtgEngineTest.UnitTests
{
    [TestClass]
    public class ConsolePlayerTests
    {
        private void testParseChoices(string userText, int minResponseCount, int maxResponseCount, int minResponseValue, int maxResponseValue, bool noDuplicates, int[] ExpectedResult)
        {
            var actualResult = ConsolePlayer.ParseChoice(userText, minResponseCount, maxResponseCount, minResponseValue, maxResponseValue, noDuplicates);
            if (ExpectedResult == null)
                Assert.IsNull(actualResult);
            else
            {
                Assert.IsNotNull(actualResult);
                Assert.AreEqual(ExpectedResult.Length, actualResult.Count);
                for (int i = 0; i < actualResult.Count; i++)
                    Assert.AreEqual(ExpectedResult[i], actualResult[i]);
            }
        }

        [TestMethod]
        public void testParseChoices_Null()
        {
            testParseChoices("", 1, 1, 1, 1, true, null);
        }

        [TestMethod]
        public void testParseChoices_1()
        {
            testParseChoices("1", 1, 1, 1, 1, true, new[] { 1 });
        }

        [TestMethod]
        public void testParseChoices_1_2_3_spaces()
        {
            testParseChoices("1 2 3", 1, int.MaxValue, 1, int.MaxValue, true, new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void testParseChoices_1_2_3_commas()
        {
            testParseChoices("1,2,3", 1, int.MaxValue, 1, int.MaxValue, true, new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void testParseChoices_1_2_3_commasSpaces()
        {
            testParseChoices("1, 2, 3", 1, int.MaxValue, 1, int.MaxValue, true, new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void testParseChoices_1_1_3_noDuplicates()
        {
            testParseChoices("1, 1, 3", 1, int.MaxValue, 1, int.MaxValue, true, null);
        }

        [TestMethod]
        public void testParseChoices_1_1_3_duplicates()
        {
            testParseChoices("1, 1, 3", 1, int.MaxValue, 1, int.MaxValue, false, new[] { 1, 1, 3 });
        }

        [TestMethod]
        public void testParseChoices_largerThanMaximum()
        {
            testParseChoices("3", 1, 1, 1, 2, false, null);
        }

        [TestMethod]
        public void testParseChoices_smallerThanMinumum ()
        {
            testParseChoices("3", 1, 1, 4, 5, false, null);
        }

        [TestMethod]
        public void testParseChoices_inappropriateResponse()
        {
            testParseChoices("Wumpus", 1, 1, 4, 5, false, null);
        }

        [TestMethod]
        public void testParseChoices_nullResponse()
        {
            testParseChoices(null, 1, 1, 4, 5, false, null);
        }
    }
}
