using System;
using System.Collections;
using System.Linq;

namespace Boggle.Models
{
    public class Board
    {
        private int diceRows = 4;
        private Die[,] dice;

        public Board(Die[,] d)
        {
            dice = d;
        }

        public Die getDie(int row, int col)
        {
            return dice[row, col];
        }

        //scrambles orientation of dice and rolls all dice,
        //then returns the 2D array of dice
        public Die[,] shakeForNewBoard()
        {
            var diceList = new ArrayList();
            for(int i = 0; i < diceRows; i++)
            {
                for(int j = 0; j < diceRows; j++)
                {
                    diceList.Add(dice[i, j]);
                }
            }

            int[] shuffledInts = generateShuffledInts(diceRows * diceRows);

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

        //returns randomly ordered n ints
        public int[] generateShuffledInts(int n)
        {
            Random random = new Random();
            int[] arr = Enumerable.Range(0, n).OrderBy(c => random.Next()).ToArray();
            return arr;
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
