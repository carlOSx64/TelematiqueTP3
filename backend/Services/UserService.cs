using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly SqliteContext context;

        public UserService(SqliteContext sqliteContext)
        {
            context = sqliteContext;
        }

        private static Random random = new Random();

        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public List<User> GetAll()
        {
            var users = context.Users
                .Include(u => u.UserGroups)
                .ToList();

            return users;
        }

        public List<Group> GetGroupsByUser(int userId)
        {
            var groups = context.Groups.Where(g => g.UserGroups.Any(ug => ug.UserId == userId)).ToList();

            return groups;
        }

        public List<Invitation> GetInvitationsByUser(int userId)
        {
            var invitations = context.Invitations.Where(i => i.UserId == userId).ToList();

            return invitations;
        }

        public void Create(string username, string password)
        {

            User user = new User()
            {
                Username = username,
                Password = password,
                ApiKey = RandomString(64)

            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        public void DeleteAll()
        {
            // Truncate table users
            var allUsers = context.Set<User>();
            context.Users.RemoveRange(allUsers);
            context.SaveChanges();
        }

        public User Authenticate(string username, string password)
        {

            try
            {
                var user = context.Users
                    .Where(s => s.Username == username)
                    .Where(s => s.Password == password)
                    .FirstOrDefault();

                return user;
            }
            catch
            {
                return null;
            }
        }

        public User GetUserByApiKey(string apiKey)
        {
            var user = context.Users
                .Where(s => s.ApiKey == apiKey)
                .FirstOrDefault();

            return user;
        }

        public bool Exists(int userId)
        {
            return context.Users.Where(u => u.Id == userId).Count() > 0;
        }        
    }
}
