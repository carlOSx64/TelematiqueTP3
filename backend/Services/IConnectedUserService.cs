using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IConnectedUserService
    {

        List<ConnectedUser> GetAll();
        void Logout(int userId);
        void Delete(int userId);
        void DeleteAll();
        void Connect(int userId, string ip);
        void Create(int userId, string ip);
        //bool Exists(int userId);
    }
}
