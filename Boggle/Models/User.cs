using System;
using System.Diagnostics.CodeAnalysis;

using System.Collections.Generic;
using Boggle.Controllers;

namespace Boggle.Models
{
    public class User
    {
        private string username;
        private List<string> wordsUsed;
        private List<string> wordsUsedOk;
        private int score;

        public User(string u)
        {
            username = u;
            wordsUsed = new List<string>();
            wordsUsedOk = new List<string>();
            score = 0;
        }

        public string getUsername()
        {
            return username;
        }

        public void setUsername(string u)
        {
            username = u;
        }

        public List<string> getWordsUsed()
        {
            return wordsUsed;
        }

        public void setScore(int s)
        {
            score = s;
        }

        public int getScore()
        {
            return score;
        }

        public void addWord(string word)
        {
            if(!isUsed(word))
                wordsUsed.Add(word);
        }

        public void updateScore(int points)
        {
            score += points;
        }

        public bool isUsed(string word)
        {
            if (wordsUsed.Contains(word))
            {
                return true;
            }
            return false;
        }

        public void addWordUsedOk(string word)
        {
            wordsUsedOk.Add(word);
            updateScore(WordValidationEngine.wordPoints(word));
        }
        public List<string> getWordsUsedOk()
        {
            return wordsUsedOk;
        }

        public void emptyWordLists()
        {
            wordsUsed.Clear();
            wordsUsedOk.Clear();
        }
    }
}
