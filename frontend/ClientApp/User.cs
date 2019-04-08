using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class User
    {
        public User(string username, bool isConnected)
        {
            Username = username;
            IsConnected = isConnected;
        }

        public string Username { get; set; }

        public bool IsConnected { get; set; }

        public string NameColor
        {
            get {
                if(IsConnected)
                    return "Green";
                return "LightGray";
            }
        }
    }
}
