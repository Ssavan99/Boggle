using System;
using System.Collections.Generic;
using System.Linq;

namespace Boggle.Models
{
    public class Server
    {
        private readonly object mtx = new object();
        private Dictionary<int, Game> games;
        private Random rnd;

        /// <summary>
        /// Upper bound on concurrent games. Game ids are drawn from a space of
        /// roughly one million, so allowing the dictionary to approach that size
        /// would make id allocation degenerate into a very long retry loop.
        /// Refusing new games well before that point keeps allocation cheap.
        /// </summary>
        public const int MaxConcurrentGames = 20000;

        private const int MaxIdAttempts = 200;

        private static Server inst = new Server();

        public static Server getInstance()
        {
            return inst;
        }

        public Server()
        {
            games = new Dictionary<int, Game>();
            rnd = new Random();
        }

        /// <summary>
        /// Creates a game, or returns null when the server is at capacity or a
        /// free id could not be found. Callers must handle null.
        /// </summary>
        public Game newGame()
        {
            lock (mtx)
            {
                if (games.Count >= MaxConcurrentGames)
                {
                    return null;
                }

                int id = 0;
                bool allocated = false;
                for (int attempt = 0; attempt < MaxIdAttempts; attempt++)
                {
                    id = rnd.Next(1, 1000000);
                    if (!games.ContainsKey(id))
                    {
                        allocated = true;
                        break;
                    }
                }

                if (!allocated)
                {
                    return null;
                }

                Game g = new Game(id, DateTime.Now);
                games[id] = g;
                return g;
            }
        }

        public Game getGame(int id)
        {
            lock (mtx)
            {
                if (games.ContainsKey(id))
                {
                    return games[id];
                }
                else
                {
                    return null;
                }
            }
        }

        public Game deleteGame(int id)
        {
            lock (mtx) {
                if (games.ContainsKey(id))
                {
                    Game g = games[id];
                    games.Remove(id);
                    return g;
                }
                else
                {
                    return null;
                }
            }
        }

        public int getGameCount()
        {
            lock (mtx)
            {
                return games.Count;
            }
        }

        /// <summary>
        /// Drops games that nobody has interacted with for longer than
        /// <paramref name="idleTimeout"/>. Games live only in memory, so without
        /// this they accumulate for the lifetime of the process. A client that is
        /// still polling keeps its game alive, so an active table is never culled.
        /// </summary>
        /// <returns>How many games were removed.</returns>
        public int removeStaleGames(TimeSpan idleTimeout)
        {
            lock (mtx)
            {
                DateTime cutoff = DateTime.UtcNow - idleTimeout;
                List<int> stale = games
                    .Where(kv => kv.Value.getLastActivityUtc() < cutoff)
                    .Select(kv => kv.Key)
                    .ToList();

                foreach (int id in stale)
                {
                    games.Remove(id);
                }

                return stale.Count;
            }
        }
    }
}
