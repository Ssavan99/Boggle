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

        public String getCoordinateUserInput()
        {
            throw new NotImplementedException();
        }

        public void getServerTime()
        {
            throw new NotImplementedException();
        }

        public void runGame()
        {
            throw new NotImplementedException();
        }

        public void increaseModelScore(User u, int amount)
        {
            game.setScoreOfUser(u, amount);
        }

        public void updateModelScore(User u, int score)
        {
            game.setScoreOfUser(u, score);
        }

        public void updateViewScore(User u, int score)
        {
            throw new NotImplementedException();
        }
    }
}
