using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ConnectedUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Ip { get; set; } // Will be useful when sending UDP messages
        public int Expiration { get; set; } // Probably won't be used
    }
}
