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
            Die d1 = new Die(new String[] { "R", "I", "F", "O", "B", "X" });
            Die d2 = new Die(new String[] { "H", "M", "S", "R", "A", "O" });
            Die d3 = new Die(new String[] { "Qu", "B", "M", "J", "O", "A" });
            Die d4 = new Die(new String[] { "E", "Z", "A", "V", "N", "D" });
            Die d5 = new Die(new String[] { "I", "F", "E", "H", "E", "Y" });
            Die d6 = new Die(new String[] { "L", "U", "P", "E", "T", "S" });
            Die d7 = new Die(new String[] { "E", "H", "I", "S", "P", "N" });
            Die d8 = new Die(new String[] { "R", "A", "L", "E", "S", "C" });
            Die d9 = new Die(new String[] { "D", "E", "N", "O", "W", "S" });
            Die d10 = new Die(new String[] { "A", "C", "I", "T", "O", "A" });
            Die d11 = new Die(new String[] { "V", "E", "T", "I", "G", "N" });
            Die d12 = new Die(new String[] { "U", "W", "I", "L", "R", "G" });
            Die d13 = new Die(new String[] { "U", "T", "O", "K", "N", "D" });
            Die d14 = new Die(new String[] { "Y", "L", "G", "K", "U", "E" });
            Die d15 = new Die(new String[] { "B", "A", "L", "I", "Y", "T" });
            Die d16 = new Die(new String[] { "P", "A", "C", "E", "M", "D" });

            Die[,] dice = new Die[,] { { d1, d2, d3, d4 }, { d5, d6, d7, d8 },
                { d9, d10, d11, d12 }, { d13, d14, d15, d16 } };

            Board b = new Board(dice);

            return b;
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
