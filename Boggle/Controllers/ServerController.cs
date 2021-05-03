using Boggle.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boggle.Controllers
{
    public class ServerController : Controller
    {
        private Server srv;
        private IActionResult gameIdNotFound, invalidUsername, usernameNotFound, gameWasEnded, emptyGuess, duplicateUsername;
        private IActionResult okMsg;

        public ServerController()
        {
            srv = Server.getInstance();
            gameIdNotFound = failedMsg("gameid not found");
            invalidUsername = failedMsg("invalid username");
            usernameNotFound = failedMsg("username not found");
            gameWasEnded = failedMsg("game was ended");
            emptyGuess = failedMsg("no word selected");
            duplicateUsername = failedMsg("Username already used");
            okMsg = Json(new { ok = true });
        }

        public Server getServer()
        {
            return srv;
        }

        private IActionResult failedMsg(string m)
        {
            return Json(new
            {
                ok = false,
                msg = m
            });
        }
        private void calcScores(Game g)
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();
            foreach (User u in g.getUsers())
            {
                foreach (string w in u.getWordsUsed())
                {
                    if (freq.ContainsKey(w)) freq[w]++;
                    else freq[w] = 1;
                }
            }
            foreach (User u in g.getUsers())
            {
                foreach (string w in u.getWordsUsed())
                {
                    if (WordDictionary.getInstance().IsWord(w) && freq[w] == 1)
                    {
                        u.addWordUsedOk(w);
                    }
                }
            }
        }
        private bool checkIsEnded(Game g)
        {
            if (g.getState() == Game.State.Playing && g.getEndTime() < DateTime.Now)
            {
                if(g.getState() != Game.State.Ended)
                {
                    calcScores(g);
                    g.updateGameLog();
                }
                g.setState(Game.State.Ended);
            }
            return g.getState() == Game.State.Ended;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult newGame()
        {
            Game g = srv.newGame();
            lock (g)
            {
                g.getBoard().shakeForNewBoard();
                return Json(new
                {
                    ok = true,
                    gameId = g.getId()
                });
            }
        }

        public IActionResult startGame(int gameId)
        {
            Game g = srv.getGame(gameId);
            lock (g)
            {
                g.resetTimer();
                g.setState(Game.State.Playing);
                return okMsg;
            }
        }

        public IActionResult getGameState(int gameId, string username)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                if (string.IsNullOrWhiteSpace(username))
                    return invalidUsername;
                User u = g.getUser(username);
                if (u == null)
                    return usernameNotFound;

                checkIsEnded(g);
                int remainingTime = (int)g.getEndTime().Subtract(DateTime.Now).TotalSeconds;
                bool ended = g.getState() == Game.State.Ended;

                int sz = g.getBoard().boardSize();
                string[][] board = new string[sz][];
                for (int i = 0; i < sz; i++)
                {
                    board[i] = new string[sz];
                    for (int j = 0; j < sz; j++)
                    {
                        board[i][j] = g.getBoard().getDie(i, j).getUpLetter();
                    }
                }

                List<string> users = g.getUsers().Select(u => u.getUsername()).ToList();
                Dictionary<string, int> userScores = new Dictionary<string, int>();
                Dictionary<string, List<string>> userGuesses = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> userGuessesOk = new Dictionary<string, List<string>>();
                foreach (var p in g.getUsers())
                {
                    int score = p.getScore();
                    List<string> wordUsed = p.getWordsUsed();
                    List<string> wordUsedOk = p.getWordsUsedOk();
                    if (!ended && p.getUsername() != u.getUsername())
                    {
                        // Mask out private data
                        score = 0;
                        wordUsed = wordUsed.Select(x => "?").ToList();
                    }
                    if (!ended)
                    {
                        wordUsedOk = new List<string>();
                    }
                    userScores.Add(p.getUsername(), score);
                    userGuesses.Add(p.getUsername(), wordUsed);
                    userGuessesOk.Add(p.getUsername(), wordUsedOk);
                }

                return Json(new
                {
                    ok = true,
                    gameId = gameId,
                    board = board,
                    users = users,
                    userScores = userScores,
                    userGuesses = userGuesses,
                    userGuessesOk = userGuessesOk,
                    startTime = g.getStartTime(),
                    state = g.getState(),
                    remainingTime = remainingTime,
                    gameLog = g.getGameLog(),
                }); ;
            }
        }

        public IActionResult login(int gameId, string username)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                if (checkIsEnded(g))
                    return gameWasEnded;
                if (string.IsNullOrWhiteSpace(username))
                    return invalidUsername;
                
                if (g.isUsernameUsed(username))
                {
                    return duplicateUsername;
                }
                User u = g.getUser(username);
                if (u == null)
                {
                    g.addPlayer(new User(username));
                }
                return okMsg;
            }
        }

        public IActionResult removePlayer(int gameId, string username)
        {
            Game g = srv.getGame(gameId);
            lock (g)
            {
                User u = g.getUser(username);
                g.removePlayer(u);
                return okMsg;
            }
        }

        public IActionResult guess(int gameId, string username, string strcoords)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                if (checkIsEnded(g))
                    return gameWasEnded;
                if (string.IsNullOrWhiteSpace(username))
                    return invalidUsername;
                User u = g.getUser(username);
                if (u == null)
                    return usernameNotFound;
                Board b = g.getBoard();

                if (strcoords == null)
                    return emptyGuess;
                int[,] coords = WordValidationEngine.generateCoordinates(strcoords);
                if (!WordValidationEngine.isValidInput(coords))
                    return failedMsg("invalid coords");
                string word = "";
                for (int i = 0; i < coords.GetLength(0); i++)
                {
                    int r = coords[i, 0];
                    int c = coords[i, 1];

                    word += b.getDie(r, c).getUpLetter();
                }

                word = word.ToLower();
                if (u.isUsed(word))
                {
                    return failedMsg("duplicated guess");
                }
                else
                {
                    u.addWord(word);
                }

                return okMsg;
            }
        }

        public IActionResult endGame(int gameId)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                if (checkIsEnded(g))
                    return gameWasEnded;
                g.setState(Game.State.Ended);
                calcScores(g);
                g.updateGameLog();
                return okMsg;
            }
        }

        public IActionResult resetGame(int gameId)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                g.resetGame();
                return okMsg;
            }
        }

        public IActionResult okMessage()
        {
            return okMsg;
        }

        public IActionResult wordScore(int gameId, string username, string word)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                User u = g.getUser(username);
                int score = 0;
                // wordsUsedOk contains only words that were not guessed by other users
                if(u.getWordsUsedOk().Contains(word))
                {
                    score = WordValidationEngine.wordPoints(word);
                }

                return Json(new
                {
                    ok = true,
                    score = score,
                });
            }
        }

        public IActionResult getGameLog(int gameId)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
                
                return Json(new
                {
                    ok = true,
                    gameLog = g.getStringGameLog(),
                });
            }
        }

    }
}
