using System;
using System.Collections;
using System.Collections.Generic;

namespace Boggle.Models
{
    public class Game
    {
        private Board board;
        private SortedDictionary<User, int> usersScores;

        public Game()
        {
            board = new Board();
        }

        public SortedDictionary<User, int> getUsersScores()
        {
            return usersScores;
        }
        public Board getBoard()
        {
            return board;
        }
        public void setBoard(Board b)
        {
            board = b;
        }
        public void setScores(SortedDictionary<User, int> us)
        {
            usersScores = us;
        }
        //public int getOneUserScore(User u)
        //{
        //    //return usersScores.TryGetValue(u);
        //}



        //public void addPlayer(User u)
        //{
        //    players.Add(u);
        //    scores.Add(0);
        //}
    }
}
