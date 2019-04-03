using Microsoft.EntityFrameworkCore;
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
            var groups = context.Groups
                .Include(g => g.UserGroups)
                .ToList();

            return groups;
        }

        public List<User> GetUsersByGroup(int groupId)
        {
            var groups = context.Users.Where(u => u.UserGroups.Any(ug => ug.GroupId == groupId)).ToList();

            return groups;
        }

        public Group Create(string name)
        {
            Group newGroup = new Group() { Name = name };

            this.context.Groups.Add(newGroup);
            this.context.SaveChanges();

            return newGroup;
        }

        public void DeleteAll()
        {
            var allGroups = context.Set<Group>();
            context.Groups.RemoveRange(allGroups);
            context.SaveChanges();
        }

        public void AddUserToGroup(int userId, int groupId, bool isAdmin)
        {
            context.UserGroup.Add(new UserGroup()
            {
                UserId = userId,
                GroupId = groupId,
                IsAdmin = isAdmin
            });
            context.SaveChanges();
        }

        public void RemoveUserFromGroup(int userId, int groupId)
        {
            UserGroup userGroup = context.UserGroup.Where(ug => ug.UserId == userId && ug.GroupId == groupId).First();
            if (userGroup != null)
            {
                context.UserGroup.Remove(userGroup);
                context.SaveChanges();
            }
        }


        public bool Exists(int groupId)
        {
            return context.Groups.Where(g => g.Id == groupId).Count() > 0;
        }
    }
}
