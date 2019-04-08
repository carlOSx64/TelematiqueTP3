using System.Collections.Generic;

namespace WebApi.Models
{
    public class UserDto
    {
        public UserDto()
        {
            this.Groups = new List<int>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public ICollection<int> Groups { get; set; }
    }
}