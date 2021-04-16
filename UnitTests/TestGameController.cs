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

            Assert.AreEqual(word1, "PURE");
            Assert.AreEqual(word2, "PUREE");
            Assert.AreEqual(word3, "DAY");
        }

        public Game makeGame(User player)
        {
            Game g = new Game();
            User u = new User("user");
            g.addPlayer(u);

                return g;
        }

        /*[TestMethod]
        public void getCoordinateUserInputTest()
        {
            User u = new User("first user");
            Game g = makeGame(u);
            List<User> users = g.getUsers();

            String s1 = "33 23 13 03";
            var stringReader = new StringReader(s1);

            GameController controller = new GameController();

            Assert.AreEqual(controller.getCoordinateUserInput(u), s1);

        }*/
    }
}
