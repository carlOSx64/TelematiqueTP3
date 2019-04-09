using System.Collections.Generic;

namespace WebApi.Models
{
    public class GroupDto
    {
        public GroupDto()
        {
            this.Members = new List<int>();
            this.Administrators = new List<int>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> Members { get; set; }
        public ICollection<int> Administrators { get; set; }
    }
}