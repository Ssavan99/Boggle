using System;
namespace Boggle.Models
{
    public class User
    {
        private String username;

        public User(String u)
        {
            username = u;
        }

        public String getUsername()
        {
            return username;
        }

        public void setUsername(String u)
        {
            username = u;
        }
    }
}
