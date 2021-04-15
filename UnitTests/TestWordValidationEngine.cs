using System;
using Boggle.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestWordValidationEngine
    {
        // helper function to compare coordinate arrays of the form
        // { {1,2}, {2, 2}, {3, 2} } etc.
        // returns true if they're equivalent, false otherwise
        public bool compareCoordArr(int[,] arr1, int[,] arr2)
        {
            if (arr1.GetLength(0) != arr2.GetLength(0) || arr1.Length != arr2.Length)
                return false;

            for(int i = 0; i < arr1.GetLength(0) - 1; i++)
            {
                if(arr1[i,0] != arr2[i,0] || arr1[i,1] != arr2[i,1])
                {
                    return false;
                }
            }
            return true;
        }

        [TestMethod]
        public void hasDuplicateCoordsTest()
        {
            int[,] coords1 = { { 1, 1 }, { 2, 2 }, { 1, 1 } };
            int[,] coords2 = { { 2, 1 }, { 3, 2 }, { 1, 1 }, { 3, 2 } };
            int[,] coords3 = { { 3, 3 }, { 0, 2 }, { 0, 0 }, {0, 3 }, { 0, 0 } };

            Assert.IsTrue(WordValidationEngine.hasDuplicateCoords(coords1));
            Assert.IsTrue(WordValidationEngine.hasDuplicateCoords(coords2));
            Assert.IsTrue(WordValidationEngine.hasDuplicateCoords(coords3));
        }

        [TestMethod]
        public void hasNoDuplicateCoordsTest()
        {
            int[,] coords1 = { { 1, 1 }, { 2, 2 }, { 1, 2 } };
            int[,] coords2 = { { 2, 1 }, { 3, 3 }, { 1, 0 }, { 3, 2 } };
            int[,] coords3 = { { 3, 3 }, { 0, 2 }, { 0, 0 }, { 0, 3 }, { 1, 0 } };

            Assert.IsFalse(WordValidationEngine.hasDuplicateCoords(coords1));
            Assert.IsFalse(WordValidationEngine.hasDuplicateCoords(coords2));
            Assert.IsFalse(WordValidationEngine.hasDuplicateCoords(coords3));
        }

        [TestMethod]
        public void generateCoordinatesTest()
        {
            int[,] coords1 = { { 1, 1 }, { 2, 2 }, { 1, 2 } };
            int[,] coords2 = { { 2, 1 }, { 3, 3 }, { 1, 0 }, { 3, 2 } };
            int[,] coords3 = { { 3, 3 }, { 0, 2 }, { 0, 0 }, { 0, 3 }, { 1, 0 } };

            string s1 = "11 22 12";
            string s2 = "21 33 10 32";
            string s3 = "33 02 00 03 10";

            Assert.IsTrue(compareCoordArr(coords1, WordValidationEngine.generateCoordinates(s1)));
            Assert.IsTrue(compareCoordArr(coords2, WordValidationEngine.generateCoordinates(s2)));
            Assert.IsTrue(compareCoordArr(coords3, WordValidationEngine.generateCoordinates(s3)));
        }

        [TestMethod]
        public void isValidInputTest()
        {
            int[,] coords1 = { { 1, 1 }, { 2, 2 }, { 1, 2 } };
            int[,] coords2 = { { 2, 2 }, { 2, 3 }, { 1, 3 }, { 0, 3 } };
            int[,] coords3 = { { 0, 0 }, { 1, 1 }, { 2, 2 }, { 3, 3 }, { 3, 2 }, { 3, 1 }, { 2, 1 }, { 1, 2 } };

            Assert.IsTrue(WordValidationEngine.isValidInput(coords1));
            Assert.IsTrue(WordValidationEngine.isValidInput(coords2));
            Assert.IsTrue(WordValidationEngine.isValidInput(coords3));
        }
        
        [TestMethod]
        public void tooShortIsValidInputTest()
        {
            int[,] coords1 = { { 1, 1 } };
            int[,] coords2 = { { 1, 1 }, { 2, 2 } };
            int[,] coords3 = { };

            Assert.IsFalse(WordValidationEngine.isValidInput(coords1));
            Assert.IsFalse(WordValidationEngine.isValidInput(coords2));
            Assert.IsFalse(WordValidationEngine.isValidInput(coords3));
        }

        [TestMethod]
        public void incorrectPathIsValidInputTest()
        {
            
            int[,] coords1 = { { 2, 1 }, { 3, 3 }, { 1, 0 }, { 3, 2 } };
            int[,] coords2 = { { 3, 3 }, { 0, 2 }, { 0, 0 }, { 0, 3 }, { 1, 0 } };
            int[,] coords3 = { { 0, 0 }, { 1, 1 }, { 2, 2 }, { 0, 3 }, { 3, 2 }, { 3, 1 }, { 2, 1 }, { 1, 2 } };

            Assert.IsFalse(WordValidationEngine.isValidInput(coords1));
            Assert.IsFalse(WordValidationEngine.isValidInput(coords2));
            Assert.IsFalse(WordValidationEngine.isValidInput(coords3));
        }

        [TestMethod]
        public void wordPointsTest()
        {
            // number corresponds with points that should be returned
            string word1 = "word";
            string word2 = "their";
            string word3 = "better";
            string word5 = "theatre";
            string word11 = "seventeen";

            Assert.AreEqual(WordValidationEngine.wordPoints(word1), 1);
            Assert.AreEqual(WordValidationEngine.wordPoints(word2), 2);
            Assert.AreEqual(WordValidationEngine.wordPoints(word3), 3);
            Assert.AreEqual(WordValidationEngine.wordPoints(word5), 5);
            Assert.AreEqual(WordValidationEngine.wordPoints(word11), 11);
        }
    }
}