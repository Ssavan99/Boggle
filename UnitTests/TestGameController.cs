using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Boggle.Controllers;
using Boggle.Models;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestGameController
    {

        [TestMethod]
        public void coordsToWordTest()
        {
            int[,] coords1 = { { 3, 3 }, { 2, 3 }, { 1, 3 }, {0, 3} };
            int[,] coords2 = { { 3, 3 }, { 2, 3 }, { 1, 3 }, { 0, 3 }, {1, 2} };
            int[,] coords3 = { { 2, 0 }, { 2, 1 }, { 3, 1 },};

            GameController controller1 = new GameController();

            String word1 = controller1.coordsToWord(coords1);
            String word2 = controller1.coordsToWord(coords2);
            String word3 = controller1.coordsToWord(coords3);

            Assert.AreEqual(word1, "pure");
            Assert.AreEqual(word2, "puree");
            Assert.AreEqual(word3, "day");
        }
    }
}
