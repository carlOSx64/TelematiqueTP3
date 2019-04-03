using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {

        List<User> GetAll();
        List<Group> GetGroupsByUser(int userId);
        void DeleteAll();
        void Create(string username, string password);
        User Authenticate(string username, string password);
        bool Exists(int userId);
    }
}
