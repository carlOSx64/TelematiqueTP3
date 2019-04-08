using System.Collections.Generic;

namespace WebApi.Models
{
    public class FileDto
    {
        public FileDto()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }
    }
}