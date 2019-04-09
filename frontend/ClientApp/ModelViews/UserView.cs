using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class UserView
    {
        public UserView(int id, string username, bool isConnected)
        {
            Id = id;
            Username = username;
            IsConnected = isConnected;
        }

        public int Id { get; }

        public string Username { get; }

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
