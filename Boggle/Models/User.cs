using System;
using System.Collections.Generic;

namespace Boggle.Models
{
    public class User
    {
        private String username;
        private List<String> wordsUsed;
        private int score;

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

        public List<String> getWordsUsed()
        {
            return wordsUsed;
        }

        public int getScore()
        {
            return score;
        }

        public void addWord(String word)
        {
            if(!isUsed(word))
                wordsUsed.Add(word);
        }

        public void updateScore(int points)
        {
            score += points;
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
