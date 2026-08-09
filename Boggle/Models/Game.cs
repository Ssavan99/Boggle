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
        private DateTime lastActivityUtc;
        private const int gameDurationSec = 3 * 60;

        private bool botEnabled;
        private Difficulty botDifficulty;
        private List<BotMove> botPlan;
        private int botCursor;

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
            lastActivityUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the game as recently used. The cleanup sweep removes games that
        /// have not been touched for a while, so every request that concerns a
        /// game should call this.
        /// </summary>
        public void touch()
        {
            lastActivityUtc = DateTime.UtcNow;
        }

        public DateTime getLastActivityUtc()
        {
            return lastActivityUtc;
        }

        public int getPlayerCount()
        {
            return users.Count;
        }

        /* ----- computer opponent ------------------------------------------- */

        public bool isBotEnabled()
        {
            return botEnabled;
        }

        public Difficulty getBotDifficulty()
        {
            return botDifficulty;
        }

        /// <summary>
        /// Adds the computer as a player. Its moves are planned later, when the
        /// round starts and the final board is known.
        /// </summary>
        public void enableBot(Difficulty difficulty)
        {
            botEnabled = true;
            botDifficulty = difficulty;
            botPlan = null;
            botCursor = 0;

            if (!users.ContainsKey(ComputerPlayer.BotName))
            {
                users.Add(ComputerPlayer.BotName, new User(ComputerPlayer.BotName));
            }
        }

        /// <summary>
        /// Solves the current board and decides what the computer will play.
        /// Called when the round starts, so the plan matches the board in front
        /// of the human.
        /// </summary>
        public void planBotMoves(Random rnd)
        {
            if (!botEnabled) return;

            List<string> solved = BoardSolver.Solve(board);
            botPlan = ComputerPlayer.BuildPlan(solved, botDifficulty, gameDurationSec, rnd);
            botCursor = 0;
        }

        public int getBotPlanCount()
        {
            return botPlan == null ? 0 : botPlan.Count;
        }

        /// <summary>
        /// Releases any computer moves that are due by <paramref name="elapsedSeconds"/>.
        /// Driven by client activity rather than a timer, so an idle game does no
        /// work at all.
        /// </summary>
        public void advanceBot(int elapsedSeconds)
        {
            if (!botEnabled || botPlan == null) return;
            if (state != State.Playing && state != State.Ended) return;

            User bot = getUser(ComputerPlayer.BotName);
            if (bot == null) return;

            while (botCursor < botPlan.Count && botPlan[botCursor].AtSecond <= elapsedSeconds)
            {
                string w = botPlan[botCursor].Word;
                if (!bot.isUsed(w))
                {
                    bot.addWord(w);
                }
                botCursor++;
            }
        }

        /// <summary>Releases every remaining computer move, for end of round.</summary>
        public void finishBot()
        {
            advanceBot(int.MaxValue);
        }

        public int getDurationSeconds()
        {
            return gameDurationSec;
        }

        public int getId()
        {
            return id;
        }
        public List<Dictionary<string, int>> getGameLog()
        {
            return gameLog;
        }
        public String getStringGameLog()
        {
            String res = "";
            foreach(Dictionary<string, int> dict in gameLog)
            {
                foreach (string user in dict.Keys)
                {
                    res += user + " " + dict[user];
                }
            }
            return res;
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
            foreach(User u in users.Values)
            {
                u.setScore(0);
                u.emptyWordLists();
            }
            board.shakeForNewBoard();
            state = State.Lobby;
            startTime = DateTime.Now;

            // The board changed, so the old plan no longer applies; it is rebuilt
            // when the next round starts.
            botPlan = null;
            botCursor = 0;
        }

        public List<String> getAllUsersInGameLog()
        {
            List<String> res = new List<String>();
            foreach(Dictionary<string, int> dict in gameLog)
            {
                foreach(String username in dict.Keys)
                {
                    if (!res.Contains(username))
                    {
                        res.Add(username);
                    }
                }
            }
            return res;

        }

        public void updateGameLog()
        {
            //stores all user:score pairs in gameLog

            Dictionary<string, int> gameScores = new Dictionary<string, int>();
            foreach (string u in users.Keys)
            {
                gameScores[u] = users[u].getScore();
            }
            gameLog.Add(gameScores);
        }
    }
}
