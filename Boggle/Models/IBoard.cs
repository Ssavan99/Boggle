using System;
namespace Boggle.Models
{
    public interface IBoard
    {
        public Die getDie(int row, int col);

        public Die[,] shakeForNewBoard();

        public String ToString();
    }
}
