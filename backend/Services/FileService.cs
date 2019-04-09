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

        public File Create(string name, string content, int groupId)
        {

            File file = new File()
            {
                Name = name,
                Content = content,
                GroupId = groupId
            };

            context.Files.Add(file);
            context.SaveChanges();
            return file;
        }

        public void Delete(string id) {
            File file = context.Files.Where(f => f.Id == Int32.Parse(id)).FirstOrDefault();
            context.Files.Remove(file);
            context.SaveChanges();
        }

        public void DeleteAll()
        {
            // Truncate table users
            var allFiles = context.Set<File>();
            context.Files.RemoveRange(allFiles);
            context.SaveChanges();
        }

    }
}
