using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class Group
    {
        public Group(string id, string name, List<UserView> users, List<UserView> admins)
        {
            Id = id;
            Name = name;
            Users = users;
            Admins = admins;
        }

        public string Id { get; }

        public string Name { get; }

        public List<UserView> Users { get; }

        public List<UserView> Admins { get; }
    }
}
