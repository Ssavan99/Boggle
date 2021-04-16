using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Boggle.Models
{
    public class Game
    {
        private Board board;
        private List<User> users;

        public Game()
        {
            board = new Board();
            users = new List<User>();
        }

        public List<User> getUsers()
        {
            return users;
        }
        public List<int> getScores()
        {
            List<int> scores = new List<int>();
            foreach (var user in users)
            {
                scores.Add(user.getScore());
            }
            return scores;
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
            return u.getScore();
        }
        public void addPlayer(User u)
        {
            users.Add(u);
        }
    }
}
