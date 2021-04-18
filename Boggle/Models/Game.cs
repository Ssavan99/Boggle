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
        private Dictionary<string, User> users;

        public Game() : this(0, DateTime.Now)
        {
        }

        public Game(int id, DateTime startTime)
        {
            this.id = id;
            this.startTime = startTime;
            board = new Board();
            users = new Dictionary<string, User>();
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
            return users.Values.ToList();
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
            //return usersData[u].getScore();
        }
        public void setScoreOfUser(User u, int score)
        {
            u.setScore(score);
        }
        public List<int> getScores()
        {
            List<int> scores = new List<int>();
            foreach (User user in users.Values)
            {
                scores.Add(user.getScore());
            }
            return scores;
        }
        public void increaseScoreOfUser(User u, int amount)
        {
            u.updateScore(amount);
        }
        public bool hasPlayer(User u)
        {
            return users.ContainsKey(u.getUsername());
        }
        public void addPlayer(User u)
        {
            users.Add(u.getUsername(), u);
        }
        public User getUser(string username)
        {
            if (users.ContainsKey(username))
            {
                return users[username];
            }
            else
            {
                return null;
            }
        }
    }
}
