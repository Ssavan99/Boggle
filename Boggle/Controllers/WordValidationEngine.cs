using System;
using System.Data;
using Boggle.Models;

namespace Boggle.Controllers
{
    public class WordValidationEngine
    {
        //checks to make sure that the same letter was not used twice
        public static bool hasDuplicateCoords(int[,] coords)
        {
            for (int i = 0; i < coords.GetLength(0); i++)
            {
                for (int j = 0; j < coords.GetLength(0); j++)
                {
                    if (coords[i, 0] == coords[j, 0])
                    {
                        if (coords[i, 1] == coords[j, 1])
                        {
                            if (i != j)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //takes in a string like "13 12 22 21" and converts it to a 2D array like
        //[[1,3], [1,2], [2,2], [2,1]]
        public static int[,] generateCoordinates(String input)
        {
            String[] stringCoords = input.Split(' ');

            int[,] coords = new int[stringCoords.Length, 2];

            for (int i = 0; i < stringCoords.Length; i++)
            {
                int r = (int)Char.GetNumericValue(stringCoords[i][0]);
                int c = (int)Char.GetNumericValue(stringCoords[i][1]);

                coords[i, 0] = r;
                coords[i, 1] = c;
            }
            return coords;
        }

        //makes sure a potential set of coordinates is at least 3 letters long,
        //makes sure all coordinates are in bounds of the board
        //finally verifies that no letter is used twice
        public static bool isValidInput(int[,] coords)
        {
            int n = coords.GetLength(0);

            if (n < 3)
                return false;

            for (int i = 0; i < n; i++)
            {
                int r = coords[i, 0];
                int c = coords[i, 1];

                if (!Board.inBounds(r, c))
                    return false;
                if (i != n - 1)
                {
                    if (!Board.adjacent(r, c, coords[i + 1, 0], coords[i + 1, 1]))
                    {
                        return false;
                    }
                }
            }

            if (!hasDuplicateCoords(coords))
            {
                return true;
            }
            else
                return false;
        }

        //returns the number of points for a word based on Boggle rules
        public static int wordPoints(String word)
        {
            int n = word.Length;
            if (n < 5)
                return 1;
            if (n == 5)
                return 2;
            if (n == 6)
                return 3;
            if (n == 7)
                return 5;
            if (n >= 8)
                return 11;

            return -1;
        }
    }
}
