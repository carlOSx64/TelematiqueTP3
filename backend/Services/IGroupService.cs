using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IGroupService
    {

        List<Group> GetAll();
        Group Create(string name);
    }
}
