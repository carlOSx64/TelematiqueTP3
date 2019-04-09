using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;

namespace WebApi.Services
{
    public class FileService : IFileService
    {
        private readonly SqliteContext context;

        public FileService(SqliteContext sqliteContext)
        {
            context = sqliteContext;
        }

        public bool Exists(int fileId)
        {
            return context.Files.Where(f => f.Id == fileId).Count() > 0;
        }

        public File Get(int fileId)
        {

            File file = context.Files.Where(f => f.Id == fileId).FirstOrDefault();

            return file;
        }

        public File Create(string name, string content, int groupId)
        {
		
	     Console.WriteLine("groupId: " + groupId);

            File file = new File()
            {
                Name = name,
                Content = content,
                GroupId = groupId
            };

            context.Files.Add(file);
            context.SaveChanges();

            NotifyClients(file.GroupId);
            return file;
        }

        public void Delete(string id)
        {
            File file = context.Files.Where(f => f.Id == Int32.Parse(id)).FirstOrDefault();
            context.Files.Remove(file);
            context.SaveChanges();

            NotifyClients(file.GroupId);
        }
        
        public void DeleteAll()
        {
            // Truncate table File
            var allFiles = context.Set<File>();
            context.Files.RemoveRange(allFiles);
            context.SaveChanges();
        }

        // Called after every file changes.
        private void NotifyClients(int groupId) {

            // From a groupId, query all users from a group
            List<User> users = context.Users.Where(u => u.UserGroups.Any(ug => ug.GroupId == groupId)).ToList();

            // J'pas familier avec LINQ, c'est peut être pas la façon la plus efficace de dire "si mon cu.Id est dans users
            List<ConnectedUser> connectedUsers = context.ConnectedUsers.Where(cu => users.Any(u => u.Id == cu.UserId)).ToList();

            // Send connected users a UDP message notifying an update
            foreach (ConnectedUser cu in connectedUsers)
            {
                try {
                    Console.WriteLine("Notifying " + cu.Ip + " over UDP");
                    // Datagram socket over UDP
                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                        ProtocolType.Udp);

                    IPAddress serverAddr = IPAddress.Parse(cu.Ip);

                    // Port open on client app
                    IPEndPoint endPoint = new IPEndPoint(serverAddr, 42069);

                    string text = "Updates available for group " + groupId;
                    byte[] send_buffer = Encoding.ASCII.GetBytes(text);

                    sock.SendTo(send_buffer , endPoint);
                }
                catch {
                    // We don't handle errors related to UDP.
                }
            }
        }
    }
}
