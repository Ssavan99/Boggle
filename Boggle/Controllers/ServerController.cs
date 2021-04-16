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
        private IActionResult gameIdNotFound;

        public ServerController ()
        {
            srv = Server.getInstance();
            gameIdNotFound = Json(new
            {
                ok = true,
                msg = "gameid not found"
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

            List<string> users = new List<string>();
            foreach(User u in g.getUsers())
            {
                users.Add(u.getUsername());
            }

            return Json(new
            {
                board = board,
                users = users,
            });
        }
    }
}
