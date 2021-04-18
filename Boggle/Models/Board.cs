using System;
using System.Collections;
using System.Collections.Generic;
using Boggle.Controllers;

namespace Boggle.Models
{
    public class Board : IBoard
    {
        private static int diceRows = 4;
        private Die[,] dice;

        public Board()
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

            dice = new Die[,] { { d1, d2, d3, d4 }, { d5, d6, d7, d8 },
                { d9, d10, d11, d12 }, { d13, d14, d15, d16 } };
        }

        public int boardSize()
        {
            return diceRows;
        }

        public void setDice(Die[,] d)
        {
            dice = d;
        }

        public Die getDie(int row, int col)
        {
            return dice[row, col];
        }

        public List<Die> getDiceAsList()
        {
            var diceList = new List<Die>();
            for (int i = 0; i < diceRows; i++)
            {
                for (int j = 0; j < diceRows; j++)
                {
                    diceList.Add(dice[i, j]);
                }
            }
            return diceList;
        }

        //scrambles orientation of dice and rolls all dice,
        //then returns the 2D array of dice
        public Die[,] shakeForNewBoard()
        {
            var diceList = getDiceAsList();

            int[] shuffledInts = Utilities.generateShuffledInts(diceRows * diceRows);

            for (int i = 0; i < diceRows; i++)
            {
                for (int j = 0; j < diceRows; j++)
                {
                    int oldDieIndex = shuffledInts[i * diceRows + j];
                    Die oldDie = (Die)diceList[oldDieIndex];
                    dice[i, j] = oldDie;
                    dice[i, j].roll();
                }
            }
            return dice;
        }

        //checks if a coordinate is a valid Die
        public static bool inBounds(int r, int c)
        {
            return (0 <= r && r < diceRows) && (0 <= c && c < diceRows);
        }

        public static bool adjacent(int r1, int c1, int r2, int c2)
        {
            bool adjRow = false;
            if (Math.Abs(r1 - r2) <= 1)
                adjRow = true;
            bool adjCol = false;
            if (Math.Abs(c1 - c2) <= 1)
                adjCol = true;
            return (adjRow && adjCol);
        }

    //returns pretty printed board
    public override String ToString()
        {
            String res = "--------------------\n";
            for(int i = 0; i < diceRows; i++)
            {
                for (int j = 0; j < diceRows; j++)
                {
                    res += "| " + dice[i, j].getUpLetter() + " |";
                }
                res += "\n--------------------\n";
            }
            return res;
        }
    }
}
