using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Boggle.Models
{
    public class Game
    {
        private int id;
        private DateTime startTime;
        private Board board;
        private SortedDictionary<User, List<string>> usersGuesses;
        private SortedDictionary<User, int> usersScores;

        public Game() : this(0, DateTime.Now)
        {
        }

        public Game(int id, DateTime startTime)
        {
            this.id = id;
            this.startTime = startTime;
            board = new Board();
            usersGuesses = new SortedDictionary<User, List<string>>();
            usersScores = new SortedDictionary<User, int>();
        }

        public int getId()
        {
            return id;
        }
        public DateTime getStartTime()
        {
            return startTime;
        }

        public List<User> getUsers()
        {
            return usersScores.Keys.ToList();
        }
        public List<int> getScores()
        {
            return usersScores.Values.ToList();
        }
        public Board getBoard()
        {
            return board;
        }
        public void setBoard(Board b)
        {
            board = b;
        }
        public int getScoreForUser(User u)
        {
            return usersScores[u];
        }
        public void setScoreOfUser(User u, int score)
        {
            usersScores[u] = score;
        }
        public void increaseScoreOfUser(User u, int amount)
        {
            usersScores[u] += amount;
        }
        public void addPlayer(User u)
        {
            usersScores.Add(u, 0);
        }

        public void userGuess(User u, string guess)
        {
            if (!usersGuesses.ContainsKey(u))
            {
                usersGuesses[u] = new List<string>();
            }
            usersGuesses[u].Add(guess);
        }
    }
}
