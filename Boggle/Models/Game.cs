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
        private SortedDictionary<User, UserData> usersData;

        public Game() : this(0, DateTime.Now)
        {
        }

        public Game(int id, DateTime startTime)
        {
            this.id = id;
            this.startTime = startTime;
            board = new Board();
            usersData = new SortedDictionary<User, UserData>();
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
            return usersData.Keys.ToList();
        }
        public SortedDictionary<User, UserData> getUsersData()
        {
            return usersData;
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
            return usersData[u].getScore();
        }
        public void setScoreOfUser(User u, int score)
        {
            usersData[u].setScore(score);
        }
        public void increaseScoreOfUser(User u, int amount)
        {
            usersData[u].addScore(amount);
        }
        public bool hasPlayer(User u)
        {
            return usersData.ContainsKey(u);
        }
        public void addPlayer(User u)
        {
            usersData.Add(u, new UserData());
        }
        public UserData getUserData(User u)
        {
            if (usersData.ContainsKey(u))
            {
                return usersData[u];
            } else
            {
                return null;
            }
        }
    }
}
