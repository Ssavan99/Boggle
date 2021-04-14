using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Boggle.Models;


namespace UnitTests
{
    [TestClass]
    public class TestBoard
    {
        public Board makeBoard()
        {
            Board b = new Board();
            return b;
        }

        [TestMethod]
        public void getDieTest()
        {
            Board b = makeBoard();
            Assert.AreEqual(b.getDie(2, 2).getUpLetter(), "V");
            Assert.AreEqual(b.getDie(3, 2).getUpLetter(), "B");
            Assert.AreEqual(b.getDie(1, 3).getUpLetter(), "R");
        }

        [TestMethod]
        public void printBoardTest()
        {
            Board b = makeBoard();
            Console.WriteLine(b);
            Assert.IsTrue(true);
        }
    }
}
