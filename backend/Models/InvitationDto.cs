using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class InvitationDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPending { get; set; }
        public int InvitedById { get; set; }
    }
}
