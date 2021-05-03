using System;
namespace Boggle.Models
{
    public class Die
    {
        private String[] faces;
        private int letterUp;
        private int numOfFaces = 6;

        public Die(String[] f)
        {
            if (f.Length == numOfFaces)
                faces = f;
            else
                throw new System.ArrayTypeMismatchException();
            letterUp = 0;
        }

        public String[] getFaces()
        {
            return faces;
        }

        public void setFaces(String[] f)
        {
            faces = f;
        }

        //changes upFace to new random face (simulates roll)
        public void roll()
        {
            Random r = new Random();
            letterUp = r.Next(0, numOfFaces);
        }

        public String getUpLetter()
        {
            return faces[letterUp];
        }
        
    }
}
