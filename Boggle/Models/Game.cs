using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Boggle.Models
{
    public class Game
    {
        public enum State
        {
            Lobby, Playing, Ended
        }

        private int id;
        private DateTime startTime;
        private Board board;
        private Dictionary<string, User> users;
        private List<Dictionary<string, int>> gameLog;
        private State state;
        private const int gameDurationSec = 3 * 60;

        public Game() : this(0, DateTime.Now)
        {
        }

        public Game(int id, DateTime startTime)
        {
            this.id = id;
            this.startTime = startTime;
            board = new Board();
            users = new Dictionary<string, User>();
            gameLog = new List<Dictionary<string, int>>();
            state = State.Lobby;
        }

        public int getId()
        {
            return id;
        }
        public DateTime getStartTime()
        {
            return startTime;
        }

        public void resetTimer()
        {
            startTime = DateTime.Now;
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
        public void removePlayer(User u)
        {
            users.Remove(u.getUsername());
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

        public bool isUsernameUsed(string username)
        {
            if (users.ContainsKey(username))
            {
                return true;
            }

            return false;
        }


        public void setState(State state)
        {
            this.state = state;
        }
        public State getState()
        {
            return state;
        }
        public DateTime getEndTime()
        {
            return startTime.AddSeconds(gameDurationSec);
        }

        public void resetGame()
        {
            //store all user:score pairs in gameLog
            Dictionary<string, int> gameScores = new Dictionary<string, int>();
            foreach(string u in users.Keys)
            {
                gameScores[u] = users[u].getScore();
            }
            gameLog.Add(gameScores);
            foreach(User u in users.Values)
            {
                u.setScore(0);
                u.emptyWordLists();
            }
            board.shakeForNewBoard();
            state = State.Lobby;
            startTime = DateTime.Now;
        }
    }
}
