using System;
using System.Collections.Generic;
using Boggle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestUser
    {
        [TestMethod]
        public void addWordTest()
        {
            string username = "user";
            string[] wordsUsed = { "hope", "you", "stub", "your", "toe" };
            User u = new User(username);

            foreach(string s in wordsUsed)
            {
                u.addWord(s);
            }

            foreach(string s in wordsUsed)
            {
                Assert.IsTrue(u.getWordsUsed().Contains(s));
            }
        }

        [TestMethod]
        public void isUsedTest()
        {
            string[] wordsUsed = { "grapefruit", "standardized", "blasphemy", "watermelon", "tweezer", "cat" };
            User u = new User("user");

            foreach(string s in wordsUsed)
            {
                u.addWord(s);
            }

            Assert.IsTrue(u.isUsed("grapefruit"));
            Assert.IsTrue(u.isUsed("blasphemy"));
            Assert.IsTrue(u.isUsed("cat"));
        }

        // test with input that was already in the list
        [TestMethod]
        public void addWordTwice()
        {
            User u = new User("user");

            u.addWord("sacriligious");
            u.addWord("sacriligious");
            u.addWord("volume");
            u.addWord("volume");

            Assert.AreEqual(u.getWordsUsed().Count, 2); // should only be two words
        }

        [TestMethod]
        public void updateScoreTest()
        {
            User u = new User("user");

            u.updateScore(40);
            Assert.AreEqual(u.getScore(), 40);

            u.updateScore(20);
            Assert.AreEqual(u.getScore(), 60);

        }

        // i hope you stub your toe
        // grapefruit, standardized, blasphemy, watermelon, tweezer, cat
        // sacriligious, euphoria, aerospace, volume, nautical, root
        // licensing, comparison, serendipitous, worm
    }
}