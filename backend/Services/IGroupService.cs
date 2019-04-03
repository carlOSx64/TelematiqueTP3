using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IGroupService
    {

        List<Group> GetAll();
        List<User> GetUsersByGroup(int groupId);
        Group Create(string name);
        void DeleteAll();
    }
}
