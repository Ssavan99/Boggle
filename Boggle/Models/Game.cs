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
      //  private SortedDictionary<User, UserData> usersData;
        private List<User> users;

        public Game() : this(0, DateTime.Now)
        {
        }

        public Game(int id, DateTime startTime)
        {
            this.id = id;
            this.startTime = startTime;
            board = new Board();
            users = new List<User>();
           // usersData = new SortedDictionary<User, UserData>();
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
            return users;
        }
        /*
        public SortedDictionary<User, UserData> getUsersData()
        {
            return usersData;
        }
        */
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
        public void increaseScoreOfUser(User u, int amount)
        {
            u.updateScore(amount);
        }
        public bool hasPlayer(User u)
        {
            return users.Exists(user => user == u);
        }
        public void addPlayer(User u)
        {
            users.Add(u);
        }
        /*
        public UserData getUserData(User u)
        {
            if (usersData.ContainsKey(u))
            {
                return usersData[u];
            } else
            {
                return null;
            }
            users.Add(u);
        }*/
    }
}
