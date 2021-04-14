using System;
using System.Collections;
using System.Collections.Generic;

namespace Boggle.Models
{
    public class Game
    {
        private Board board;
        private List<int> scores;
        private List<User> players;

        public Game()
        {
            board = new Board();
        }

        public List<int> getScores()
        {
            return scores;
        }
        public List<User> getPlayers()
        {
            return players;
        }
        public Board getBoard()
        {
            return board;
        }
        public void setBoard(Board b)
        {
            board = b;
        }
        public void setScores(List<int> s)
        {
            scores = s;
        }
        public void setPlayers(List<User> p)
        {
            players = p;
        }
        public void addPlayer(User u)
        {
            players.Add(u);
            scores.Add(0);
        }
    }
}
