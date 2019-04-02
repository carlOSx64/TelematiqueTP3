using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public class GroupService : IGroupService
    {
        private readonly SqliteContext context;

        public GroupService(SqliteContext sqliteContext)
        {
            this.context = sqliteContext;
        }

        public List<Group> GetAll()
        {
            return this.context.Groups.ToList();
        }

        public Group Create(string name)
        {
            Group newGroup = new Group() { Name = name };

            this.context.Groups.Add(newGroup);
            this.context.SaveChanges();

            return newGroup;
        }
    }
}
