using System.Collections.Generic;

namespace WebApi.Models
{
    public class Group
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserGroup> Members { get; set; }
        //public List<User> Administrators { get; set; }
    }
}