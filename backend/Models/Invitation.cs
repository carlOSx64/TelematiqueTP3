using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Invitation
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPending { get; set; }
        public int InvitedById { get; set; }
        public User InvitedBy { get; set; }
    }
}
