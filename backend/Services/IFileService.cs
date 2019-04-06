using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IFileService
    {
        File Create(string name, string content);
        void Delete(string id);
    }
}
