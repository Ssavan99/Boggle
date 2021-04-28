using System;
using System.Collections.Generic;
using System.Text;
using Boggle.Controllers;
using Boggle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestServerController : Controller
    {
        private IActionResult okMsg;
        private Dictionary<int, Game> games;
        public TestServerController()
        {
            //srv = Server.getInstance();
            okMsg = Json(new { ok = true });
            games = new Dictionary<int, Game>();
        }

        /*[TestMethod]
        public void newGameTest()
        {
            ServerController controller1 = new ServerController();
            Server server = new Server();

            var game1 = controller1.newGame();
            

            Assert.AreEqual(game1, Json(new
            {
                ok = true,
                gameId = controller1.getServer().
            }));
        }*/


        [TestMethod]
        public void startGameTest()
        {
            ServerController controller1 = new ServerController();

            int id = controller1.getServer().newGame().getId();

            var game1 = controller1.startGame(id);

            Assert.AreEqual(game1, controller1.okMessage());
        }

        [TestMethod]
        public void loginTest()
        {
            ServerController controller1 = new ServerController();

            var id = controller1.getServer().newGame().getId();
            String username = "Sam";

            Assert.AreEqual(controller1.login(id, username), controller1.okMessage());
        }

        [TestMethod]
        public void removePlayerTest()
        {
            ServerController controller1 = new ServerController();

            var id = controller1.getServer().newGame().getId();

            String username = "Sam";
            User u = new User(username);

            controller1.getServer().getGame(id).addPlayer(u);

            Assert.AreEqual(controller1.removePlayer(id, username), controller1.okMessage());
        }

        [TestMethod]
        public void guessTest()
        {
            ServerController controller1 = new ServerController();

            var id = controller1.getServer().newGame().getId();
            
            String username = "Sam";
            User u = new User(username);
            controller1.getServer().getGame(id).addPlayer(u);

            string strcoords = "33 23 13 03";

            Assert.AreEqual(controller1.guess(id,username,strcoords), controller1.okMessage());
        }

        [TestMethod]
        public void endGameTest()
        {
            ServerController controller1 = new ServerController();

            int id = controller1.getServer().newGame().getId();
            
            controller1.startGame(id);
            var game1 = controller1.endGame(id);
            
            Assert.AreEqual(game1, controller1.okMessage());
        }

        [TestMethod]
        public void resetGameTest()
        {
            ServerController controller1 = new ServerController();

            int id = controller1.getServer().newGame().getId(); 

            controller1.startGame(id);
            var game1 = controller1.resetGame(id);

            Assert.AreEqual(game1,controller1.okMessage());
        }
    }
}
