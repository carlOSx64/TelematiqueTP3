using System.Collections.Generic;

namespace WebApi.Models
{
    public class User
    {
        public User()
        {
            this.UserGroups = new List<UserGroup>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}