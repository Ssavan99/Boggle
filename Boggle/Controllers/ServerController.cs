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
        private IActionResult gameIdNotFound, usernameNotFound;
        private IActionResult okMsg;

        public ServerController()
        {
            srv = Server.getInstance();
            gameIdNotFound = failedMsg("gameid not found");
            usernameNotFound = failedMsg("username not found");
            okMsg = Json(new { ok = true });
        }

        public IActionResult failedMsg(string m)
        {
            return Json(new
            {
                ok = true,
                msg = m
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult newGame()
        {
            Game g = srv.newGame();
            g.getBoard().shakeForNewBoard();
            return Json(new
            {
                ok = true,
                gameId = g.getId()
            });
        }

        public IActionResult getGameState(int gameId)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
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

            Dictionary<string, int> userScores = new Dictionary<string, int>();
            foreach (var p in g.getUsersData())
            {
                userScores.Add(p.Key.getUsername(), p.Value.getScore());
            }

            DateTime endTime = g.getStartTime().AddMinutes(3);
            DateTime now = DateTime.Now;
            bool ended = now > endTime;

            return Json(new
            {
                board = board,
                users = userScores,
                startTime = g.getStartTime(),
                endTime = endTime,
                ended = ended,
            });
        }

        public IActionResult login(int gameId, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return failedMsg("invalid username");
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;

            User u = new User(username);
            if (!g.hasPlayer(u))
            {
                g.addPlayer(u);
            }
            return okMsg;
        }

        public IActionResult guess(int gameId, string username, string strcoords)
        {
            Game g = srv.getGame(gameId);
            if (g == null) return gameIdNotFound;
            User u = new User(username);
            UserData ud = g.getUserData(u);
            if (ud == null) return usernameNotFound;
            Board b = g.getBoard();

            int[,] coords = WordValidationEngine.generateCoordinates(strcoords);
            if (!WordValidationEngine.isValidInput(coords))
                return failedMsg("invalid coords");
            String word = "";
            for (int i = 0; i < coords.GetLength(0); i++)
            {
                int r = coords[i, 0];
                int c = coords[i, 1];

                word += b.getDie(r, c).getUpLetter();
            }

            word = word.ToLower();
            if(!ud.addGuess(word))
            {
                return failedMsg("duplicated guess");
            }

            return okMsg;
        }
    }
}
