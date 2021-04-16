using System;
using System.Collections.Generic;
using Boggle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestGame
    {
        // helper function to initialize a game with Users passed in arr
        public Game makeGame(User[] arr)
        {
            Game g = new Game();

            foreach (User u in arr)
            {
                g.addPlayer(u);
            }

            return g;
        }
        
        // by virtue of class design this test is used to test both addPlayer
        // and getUsers simultaneously
        [TestMethod]
        public void addPlayerGetUsersTest()
        {
            User[] arr = { new User("first user"), new User("second user"), new User("third user") };

            Game g = makeGame(arr);

            List<User> users = g.getUsers();

            Assert.IsTrue(users.Contains(arr[0]));
            Assert.IsTrue(users.Contains(arr[1]));
            Assert.IsTrue(users.Contains(arr[2]));
        }

        [TestMethod]
        public void getScoresTest()
        {
            User[] arr = { new User("first user"), new User("second user"), new User("third user") };

            Game g = makeGame(arr);

            g.getUsers()[0].updateScore(10);
            g.getUsers()[1].updateScore(20);
            g.getUsers()[2].updateScore(30);

            List<int> scores = g.getScores();

            Assert.AreEqual(scores[0], 10);
            Assert.AreEqual(scores[1], 20);
            Assert.AreEqual(scores[2], 30);
        }
    }
}
