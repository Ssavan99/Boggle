using System;
using System.Collections.Generic;

namespace Boggle.Models
{
    public class User
    {
        private String username;
        public List<String> wordsUsed { get; set; }
        public int score { get; set; }

        public User(String u)
        {
            username = u;
            wordsUsed = new List<String>();
            score = 0;
        }

        public String getUsername()
        {
            return username;
        }

        public void setUsername(String u)
        {
            username = u;
        }

        public void addWord(String word)
        {
            wordsUsed.Add(word);
        }

        public void updateScore(int points)
        {
            score = score + points;
        }

        public bool isUsed(String word)
        {
            if (wordsUsed.Contains(word))
            {
                return true;
            }
            return false;
        }
    }
}
