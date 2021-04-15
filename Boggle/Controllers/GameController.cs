using System;
using Boggle.Models;

namespace Boggle.Controllers
{
    public class GameController : IGameController
    {
        Game game;
        WordDictionary dict;

        public GameController()
        {
            dict = new WordDictionary();
        }

        //attempts to get points for an input from a user
        //if the input is worth points, increases the points in the model
        //and triggers an update to make the view reflect the change
        public void attemptWord(User u, String input)
        {
            Board b = game.getBoard();
            int[,] coords = WordValidationEngine.generateCoordinates(input);
            if (!WordValidationEngine.isValidInput(coords))
                return;
            String word = "";
            for (int i = 0; i < coords.Length; i++)
            {
                int r = coords[i, 0];
                int c = coords[i, 1];

                word += b.getDie(r, c).getUpLetter();
            }

            if (dict.IsWord(word))
            {
                int points = WordValidationEngine.wordPoints(word);
                increaseModelScore(u, points);
                //update view score according to model score

            }

        }

        //gets any input from a user as a string
        public String getCoordinateUserInput(User u)
        {
            throw new NotImplementedException();
        }


        public void getServerTime()
        {
            throw new NotImplementedException();
        }

        
        public void runGame()
        {

            //while (timer is running)
            {
                foreach (User u in game.getUsers())
                {
                    String input = getCoordinateUserInput(u);
                    attemptWord(u, input);
                }
            }
        }

        //increases the score of a user in the model by an amount
        public void increaseModelScore(User u, int amount)
        {
            game.setScoreOfUser(u, amount);
        }

        public void setModelScore(User u, int score)
        {
            game.setScoreOfUser(u, score);
        }

        public void setViewScore(User u, int score)
        {
            throw new NotImplementedException();
        }
    }
}
