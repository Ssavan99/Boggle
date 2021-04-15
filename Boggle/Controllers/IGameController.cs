using System;
using Boggle.Models;

namespace Boggle.Controllers
{
    public interface IGameController
    {
        public String getCoordinateUserInput(User u);

        public void setModelScore(User u, int score);

        public void setViewScore(User u, int score);

        public void attemptWord(User u, String input);

        public void runGame();

        public void getServerTime();
    }
}
