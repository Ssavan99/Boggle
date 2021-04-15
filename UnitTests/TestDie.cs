using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Boggle.Models;


namespace UnitTests
{
    [TestClass]
    public class TestDie
    {
        [TestMethod]
        public void makeDieTest()
        {
            String[] f = { "R", "I", "F", "O", "B", "X" };
            Die d = new Die(f);

            Assert.IsTrue("X".Equals(d.getFaces()[5]));
            Assert.IsTrue("O".Equals(d.getFaces()[3]));
            Assert.IsTrue("R".Equals(d.getFaces()[0]));
            Assert.IsTrue("B".Equals(d.getFaces()[4]));
            Assert.IsTrue("I".Equals(d.getFaces()[1]));
            Assert.IsTrue("F".Equals(d.getFaces()[2]));

        }

        [TestMethod]
        [ExpectedException(typeof(ArrayTypeMismatchException))]
        public void badDieInputTest()
        {
            String[] f = { "R", "I", "F", "O", "B", "X", "T" };
            Die d = new Die(f);
        }

        [TestMethod]
        public void rollTest()
        {
            String[] f = { "R", "I", "F", "O", "B", "X" };
            Die d = new Die(f);
            d.roll();
            String result = d.getUpLetter();

            Assert.IsTrue(result.Equals("R") || result.Equals("I") ||
                result.Equals("F") || result.Equals("O") || result.Equals("B") ||
                result.Equals("X"));
        }
    }
}
