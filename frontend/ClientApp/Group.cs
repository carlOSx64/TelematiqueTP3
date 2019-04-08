using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class Group
    {
        public Group(string id, string name, List<User> users, List<User> admins)
        {
            Id = id;
            Name = name;
            Users = users;
            Admins = admins;
        }

        public string Id { get; }

        public string Name { get; }

        public List<User> Users { get; }

        public List<User> Admins { get; }
    }
}
