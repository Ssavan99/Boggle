using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boggle.Models
{
    public class Server
    {
        private readonly object mtx = new object();
        private Dictionary<int, Game> games;
        private Random rnd;

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

        public Game newGame()
        {
            lock (mtx)
            {
                int id;
                do id = rnd.Next(1, 1000000);
                while (games.ContainsKey(id));

                id = 123;
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
    }
}
