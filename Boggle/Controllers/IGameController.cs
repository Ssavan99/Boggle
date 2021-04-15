using System;
using Boggle.Models;

namespace Boggle.Controllers
{
    public interface IGameController
    {
        public String getCoordinateUserInput();

        public void updateModelScore(User u, int score);

        public void updateViewScore(User u, int score);

        public void attemptWord(User u, String input);

        public void runGame();

        public void getServerTime();
    }
}
