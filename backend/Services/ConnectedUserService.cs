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
    public class ConnectedUserService : IConnectedUserService
    {
        private readonly SqliteContext context;

        public ConnectedUserService(SqliteContext sqliteContext)
        {
            context = sqliteContext;
        }

        public List<ConnectedUser> GetAll()
        {
            var connectedUsers = context.ConnectedUsers
                .ToList();

            return connectedUsers;
        }

        public void Connect(int userId, string ip) {
            Create(userId, ip);
        }

        public void Create(int userId, string ip)
        {

            ConnectedUser connectedUser = new ConnectedUser()
            {
                UserId = userId,
                Ip = ip
            };

            var row = context.ConnectedUsers.SingleOrDefault(c => c.UserId == userId);

            // If already connected, update IP
            if (row != null) {
                row.Ip = ip;
            } else {
                context.ConnectedUsers.Add(connectedUser);
            }
            
            context.SaveChanges();
        }

        public void Delete(int userId) {
            ConnectedUser connectedUser = context.ConnectedUsers.Where(c => c.UserId == userId).FirstOrDefault();
            context.ConnectedUsers.Remove(connectedUser);
            context.SaveChanges();
        }

        public void Logout(int userId) {
            Delete(userId);
        }

        public void DeleteAll()
        {
            // Truncate table ConnectedUsers
            var allConnectedUsers = context.Set<ConnectedUser>();
            context.ConnectedUsers.RemoveRange(allConnectedUsers);
            context.SaveChanges();
        }

        public bool Exists(int userId)
        {
            return context.ConnectedUsers.Where(c => c.UserId == userId).Count() > 0;
        }        
    }
}
