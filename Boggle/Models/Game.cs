using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Boggle.Models
{
    public class Game
    {
        private Board board;
        private SortedDictionary<User, int> usersScores;

        public Game()
        {
            board = new Board();
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
        public void addPlayer(User u)
        {
            usersScores.Add(u, 0);
        }
    }
}
