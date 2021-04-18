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
        private IActionResult gameIdNotFound, invalidUsername, usernameNotFound, gameWasEnded;
        private IActionResult okMsg;

        public ServerController()
        {
            srv = Server.getInstance();
            gameIdNotFound = failedMsg("gameid not found");
            invalidUsername = failedMsg("invalid username");
            usernameNotFound = failedMsg("username not found");
            gameWasEnded = failedMsg("game was ended");
            okMsg = Json(new { ok = true });
        }

        private IActionResult failedMsg(string m)
        {
            return Json(new
            {
                ok = true,
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
            if (!g.isEnded() && g.getEndTime() < DateTime.Now)
            {
                g.setEnded(true);
                calcScores(g);
            }
            return g.isEnded();
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
                //g.getBoard().shakeForNewBoard();
                return Json(new
                {
                    ok = true,
                    gameId = g.getId()
                });
            }
        }

        public IActionResult getGameState(int gameId)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            lock (g)
            {
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
                bool ended = g.isEnded();

                List<string> users = g.getUsers().Select(u => u.getUsername()).ToList();
                Dictionary<string, int> userScores = new Dictionary<string, int>();
                Dictionary<string, List<string>> userGuesses = new Dictionary<string, List<string>>();
                if (ended)
                {
                    foreach (var p in g.getUsers())
                    {
                        userScores.Add(p.getUsername(), p.getScore());
                        userGuesses.Add(p.getUsername(), p.getWordsUsed());
                    }
                }

                return Json(new
                {
                    board = board,
                    users = users,
                    userScores = userScores,
                    userGuesses = userGuesses,
                    startTime = g.getStartTime(),
                    ended = ended,
                });
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
                User u = g.getUser(username);
                if (u == null)
                {
                    g.addPlayer(new User(username));
                }
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
                g.setEnded(true);
                calcScores(g);
                return okMsg;
            }
        }

    }
}
