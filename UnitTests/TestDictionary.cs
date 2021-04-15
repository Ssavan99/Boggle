using Boggle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class TestDictionary
    {
        [TestMethod]
        public void CreateDictTest()
        {
            WordDictionary dict = new WordDictionary();
            int actualCount = 194433;
            List<String> dictWords = dict.DictionaryWords;
            int count = dictWords.Count;
            Assert.IsTrue(actualCount.Equals(count));
            Assert.IsTrue('a'.Equals(dictWords[0][0]));
        }

        [TestMethod]
        public void IsWordCheckTest()
        {
            WordDictionary dict = new WordDictionary();
            string word = "gobble";
            Assert.IsTrue(dict.IsWord(word));
            word = "lalalalalallal";
            Assert.IsFalse(dict.IsWord(word));
        }
        
    }

}
 
