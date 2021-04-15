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

        // tests that getScores returns and that unaltered values are 0
        [TestMethod]
        public void getScoresTest()
        {
            User[] users = { new User("first user"), new User("second user"), new User("third user") };
            Game g = makeGame(users);

            List<int> scores = g.getScores();

            foreach(int s in scores)
            {
                Assert.AreEqual(0, s);
            }
        }

        // by virtue of class design this also tests the getScoreForUser function
        [TestMethod]
        public void increaseScoreOfUserTest()
        {
            User[] users = { new User("first user"), new User("second user"), new User("third user"), new User("fourth user") };
            Game g = makeGame(users);

            g.increaseScoreOfUser(users[0], 30);
            g.increaseScoreOfUser(users[1], 10);
            g.increaseScoreOfUser(users[2], 20);
            g.increaseScoreOfUser(users[3], 1);

            Assert.AreEqual(g.getScoreForUser(users[0]), 30);
            Assert.AreEqual(g.getScoreForUser(users[1]), 10);
            Assert.AreEqual(g.getScoreForUser(users[2]), 20);
            Assert.AreEqual(g.getScoreForUser(users[3]), 1);

            g.increaseScoreOfUser(users[0], 1);
            g.increaseScoreOfUser(users[1], 90);

            Assert.AreEqual(g.getScoreForUser(users[0]), 31);
            Assert.AreEqual(g.getScoreForUser(users[1]), 100);
        }
    }
}
