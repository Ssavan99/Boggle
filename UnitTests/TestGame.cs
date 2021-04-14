using System;
using Boggle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestGame
    {
        [TestMethod]
        public void getBoardTest()
        {
            Game game = new Game();
            Board b = new Board();
            Assert.AreEqual(game.Board)
        }
    }
}
