﻿using System.Collections.Generic;

namespace WebApi.Models
{
    public class Group
    {
        public Group()
        {
            this.UserGroups = new List<UserGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> files { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}