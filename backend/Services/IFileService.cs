using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IFileService
    {
        File Get(int fileId);
        File Create(string name, string content, int groupId);
        void Delete(string id);

        void DeleteAll();
    }
}
