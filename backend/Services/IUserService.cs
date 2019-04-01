using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {

        List<User> getAllUsers();
        void deleteAll();
        void add(string username, string password);
        User authenticate(string username, string password);
    }
}
