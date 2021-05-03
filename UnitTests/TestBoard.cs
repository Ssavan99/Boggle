using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Boggle.Models;
using System.Collections.Generic;

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

        // helper function to compare the faces of two rolled die
        // returns true if the faces are different
        public bool uniqueDie(Die d1, Die d2)
        {
            bool found = false;
            for (int i = 0; i < d1.getFaces().Length; i++)
            {
                for (int j = 0; j < d2.getFaces().Length; j++)
                {
                    if(d1.getFaces()[i] == d2.getFaces()[j])
                    {
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    // the die are different, exit the loop
                    break;
                } else
                {
                    found = false;
                }
            }

            return !found;
        }

        [TestMethod]
        public void getDieTest()
        {
            Board b = makeBoard();

            Assert.AreEqual(b.getDie(2, 2).getUpLetter(), "V");
            Assert.AreEqual(b.getDie(3, 2).getUpLetter(), "B");
            Assert.AreEqual(b.getDie(1, 3).getUpLetter(), "R");
        }

        // confirms there are no repeat die on the board
        [TestMethod]
        public void shakeForNewBoardTest()
        {
            Board b = makeBoard();
            b.setDice(b.shakeForNewBoard());
            List<Die> list = b.getDiceAsList();
            bool repeats = false;

            for(int i = 0; i < list.Count - 1; i++)
            {
                for(int j = i + 1; j < list.Count; j++)
                {
                    // if the die are equivalent, test fails
                    if(!uniqueDie(list[i], list[j]))
                    {
                        repeats = true;
                        break;
                    }
                }
                if(repeats)
                {
                    break;
                }
            }

            Assert.IsFalse(repeats);
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
