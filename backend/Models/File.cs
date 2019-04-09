using System.Collections.Generic;

namespace WebApi.Models
{
    public class File
    {
        public File()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Group Group {get; set;}
        public int GroupId {get; set;}
    }
}