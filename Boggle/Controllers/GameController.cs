using System;
using System.Timers;
using Boggle.Models;

namespace Boggle.Controllers
{
    public class GameController : IGameController
    {
        private Game game;
        private Timer timer;
        private WordDictionary dict;
        bool running;

        public GameController()
        {
            game = new Game();
            dict = new WordDictionary();
            timer = new Timer();
            running = false;
        }

        //attempts to get points for an input from a user
        //if the input is worth points, increases the points in the model
        //and triggers an update to make the view reflect the change
        //returns the word if valid
        public String attemptWord(User u, String input)
        {
            int[,] coords = WordValidationEngine.generateCoordinates(input);
            if (!WordValidationEngine.isValidInput(coords))
                return null;
            String word = coordsToWord(coords);

            if (dict.IsWord(word))
            {
                if (u.isUsed(word))
                {
                    Console.WriteLine("You have used the word!");
                    return null;
                }
                int points = WordValidationEngine.wordPoints(word);
                u.addWord(word);
                u.updateScore(points);
                //increaseModelScore(u, points);
                //update view score according to model score
            }

            return word;

        }

        public String coordsToWord(int[,] coords)
        {
            Board b = game.getBoard();
            String word = "";
            for (int i = 0; i < coords.GetLength(0); i++)
            {
                int r = coords[i, 0];
                int c = coords[i, 1];

                word += b.getDie(r, c).getUpLetter();
            }

            word = word.ToLower();
            return word;
        }

        //gets any input from a user as a string
        public String getCoordinateUserInput(User u)
        {
            String input = Console.ReadLine();
            return input;
        }


        public void getServerTime()
        {
            throw new NotImplementedException();
        }

        
        public void runGame(User u)
        {
            //while (timer is running)
            //{
            //    foreach (User u in game.getUsers())
            //    {
            //        String input = getCoordinateUserInput(u);
            //        attemptWord(u, input);
            //    }
            //}
            game.addPlayer(u);
            //game.getBoard().shakeForNewBoard();
            running = true;
            while (running)
            {
                Console.WriteLine(game.getBoard());
                Console.WriteLine("User has score: " + game.getScoreForUser(u));

                String input = getCoordinateUserInput(u);
                String word = attemptWord(u, input);
                if (word != null)
                    Console.WriteLine(word);
                if (input.Equals("quit"))
                    running = false;
            }
        }

        //increases the score of a user in the model by an amount
        //public void increaseModelScore(User u, int amount)
        //{
          //  game.increaseScoreOfUser(u, amount);
        //}
        /*
        public void setModelScore(User u, int score)
        {
            game.setScoreOfUser(u, score);
        }
        */
        public void setViewScore(User u, int score)
        {
            throw new NotImplementedException();
        }
    }
}
