using System;
using Boggle.Models;

namespace Boggle.Controllers
{
    public class WordValidationEngine
    {
        public static bool hasDuplicateCoords(int[,] coords)
        {
            for (int i = 0; i < coords.Length; i++)
            {
                for (int j = 0; j < coords.Length; j++)
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

        public static bool isValidInput(int[,] coords)
        {
            int n = coords.Length;

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
    }
}
