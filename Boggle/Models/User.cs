using System;
using System.Diagnostics.CodeAnalysis;

namespace Boggle.Models
{
    public class User : IComparable<User>
    {
        private String username;

        public User(String u)
        {
            username = u;
        }

        public int CompareTo([AllowNull] User other)
        {
            if (other == null) return username.CompareTo(null);
            else return username.CompareTo(other.username);
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
