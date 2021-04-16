using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boggle.Models
{
    public class UserData
    {
        int score;
        List<string> guesses;

        public UserData()
        {
            score = 0;
            guesses = new List<string>();
        }

        public List<string> getGuesses()
        {
            return guesses;
        }
        public void addGuess(string guess)
        {
            guesses.Add(guess);
        }

        public int getScore()
        {
            return score;
        }
        public void addScore(int add)
        {
            score += add;
        }
    }
}
