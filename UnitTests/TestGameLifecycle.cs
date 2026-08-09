using System;
using System.Reflection;
using Boggle.Controllers;
using Boggle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    /// Games are held in memory and used to accumulate forever: deleteGame was
    /// never called from anywhere. These cover the reclamation paths.
    /// </summary>
    [TestClass]
    public class TestGameLifecycle
    {
        /// <summary>
        /// Backdates a game's last-activity stamp so idle behaviour can be tested
        /// without waiting. The field is private because nothing outside the game
        /// should be moving this clock in production code.
        /// </summary>
        private static void backdate(Game g, TimeSpan age)
        {
            FieldInfo f = typeof(Game).GetField("lastActivityUtc",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(f, "lastActivityUtc field is missing from Game");
            f.SetValue(g, DateTime.UtcNow - age);
        }

        [TestMethod]
        public void newGameIsTracked()
        {
            Server srv = new Server();
            int before = srv.getGameCount();
            srv.newGame();
            Assert.AreEqual(before + 1, srv.getGameCount());
        }

        [TestMethod]
        public void deleteGameRemovesIt()
        {
            Server srv = new Server();
            Game g = srv.newGame();

            srv.deleteGame(g.getId());

            Assert.AreEqual(0, srv.getGameCount());
            Assert.IsNull(srv.getGame(g.getId()));
        }

        [TestMethod]
        public void idleGamesAreSweptAway()
        {
            Server srv = new Server();
            Game stale = srv.newGame();
            Game active = srv.newGame();

            backdate(stale, TimeSpan.FromHours(2));

            int removed = srv.removeStaleGames(TimeSpan.FromMinutes(30));

            Assert.AreEqual(1, removed);
            Assert.IsNull(srv.getGame(stale.getId()));
            Assert.IsNotNull(srv.getGame(active.getId()));
        }

        [TestMethod]
        public void activeGamesSurviveTheSweep()
        {
            Server srv = new Server();
            Game g = srv.newGame();

            backdate(g, TimeSpan.FromHours(2));
            g.touch();

            Assert.AreEqual(0, srv.removeStaleGames(TimeSpan.FromMinutes(30)));
            Assert.IsNotNull(srv.getGame(g.getId()));
        }

        [TestMethod]
        public void lastPlayerLeavingDropsTheGame()
        {
            ServerController c = new ServerController();
            Server srv = c.getServer();
            int id = srv.newGame().getId();

            c.login(id, "solo");
            Assert.IsNotNull(srv.getGame(id));

            c.removePlayer(id, "solo");

            Assert.IsNull(srv.getGame(id),
                "the game should be dropped once the last player leaves");
        }

        [TestMethod]
        public void gameSurvivesWhileOtherPlayersRemain()
        {
            ServerController c = new ServerController();
            Server srv = c.getServer();
            int id = srv.newGame().getId();

            c.login(id, "alice");
            c.login(id, "bob");

            c.removePlayer(id, "alice");

            Assert.IsNotNull(srv.getGame(id),
                "bob is still playing, so the game must stay");
        }

        [TestMethod]
        public void removePlayerOnUnknownGameDoesNotThrow()
        {
            ServerController c = new ServerController();

            // Previously this dereferenced a null game and threw.
            var result = c.removePlayer(999999999, "nobody");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void startGameOnUnknownGameDoesNotThrow()
        {
            ServerController c = new ServerController();

            var result = c.startGame(999999999);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void gameIdsStayUniqueAcrossManyGames()
        {
            Server srv = new Server();
            var seen = new System.Collections.Generic.HashSet<int>();

            for (int i = 0; i < 500; i++)
            {
                Game g = srv.newGame();
                Assert.IsNotNull(g, "allocation should succeed well below capacity");
                Assert.IsTrue(seen.Add(g.getId()), "duplicate game id issued");
            }

            Assert.AreEqual(500, srv.getGameCount());
        }
    }
}
