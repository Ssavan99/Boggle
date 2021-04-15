using System;
namespace Boggle.Models
{
    public interface IDie
    {
        public String[] getFaces();

        public void setFaces(String[] f);

        public void roll();
        
        public String getUpLetter();
    }
}
