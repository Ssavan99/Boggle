using System;
using System.Linq;

namespace Boggle.Controllers
{
    public class Utilities
    {
        //returns randomly ordered n ints
        public static int[] generateShuffledInts(int n)
        {
            Random random = new Random();
            int[] arr = Enumerable.Range(0, n).OrderBy(c => random.Next()).ToArray();
            return arr;
        }
    }
}
