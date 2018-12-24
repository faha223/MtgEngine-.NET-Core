using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;

namespace MtgEngine.UnitTests
{
    [TestClass]
    public class ManaParserTests
    {
        private void testManaParser(string testString, ManaAmount[] expectedResult)
        {
            var actualResult = ManaParser.Parse(testString);
            if (expectedResult == null)
                Assert.IsNull(actualResult);
            else
            {
                Assert.IsNotNull(actualResult);
                Assert.AreEqual(expectedResult.Length, actualResult.Length);
                for (int i = 0; i < actualResult.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Amount, actualResult[i].Amount);
                    Assert.AreEqual(expectedResult[i].Color, actualResult[i].Color);
                }
            }
        }

        [TestMethod]
        public void Test1G()
        {
            testManaParser("{1}{G}", new[] { new ManaAmount(1, ManaColor.Generic), new ManaAmount(1, ManaColor.Green) });
        }

        [TestMethod]
        public void Test1BB()
        {
            testManaParser("{1}{B}{B}", new[] { new ManaAmount(1, ManaColor.Generic), new ManaAmount(2, ManaColor.Black) });
        }

        [TestMethod]
        public void TestWUBRG()
        {
            testManaParser("{W}{U}{B}{R}{G}", new[]
            {
                new ManaAmount(1, ManaColor.White),
                new ManaAmount(1, ManaColor.Blue),
                new ManaAmount(1, ManaColor.Black),
                new ManaAmount(1, ManaColor.Red),
                new ManaAmount(1, ManaColor.Green)
            });
        }

        [TestMethod]
        public void TestPR()
        {
            testManaParser("{R/P}", new[] { new ManaAmount(1, ManaColor.PhyrexianRed) });
        }

        [TestMethod]
        public void Test2W()
        {
            testManaParser("{2/W}", new[] { new ManaAmount(1, ManaColor.TwoOrWhite) });
        }

        [TestMethod]
        public void TestXXX()
        {
            testManaParser("{X}{X}{X}", new[] { new ManaAmount(3, ManaColor.GenericX) });
        }

        [TestMethod]
        public void Test0()
        {
            testManaParser("{0}", null);
        }
    }
}
