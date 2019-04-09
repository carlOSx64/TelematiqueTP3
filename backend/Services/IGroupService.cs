using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IGroupService
    {

        Group Get(int groupId);
        List<Group> GetAll();
        List<User> GetUsersByGroup(int groupId);
        Group Create(string name);
        void DeleteAll();
        void AddUserToGroup(int userId, int groupId, bool isAdmin);
        void RemoveUserFromGroup(int userId, int groupId);
        void EditUserPermissions(int userId, int groupId, bool isAdmin);
        void InviteUser(int userId, int groupId, bool isAdmin, int invitedBy);
        void UpdateInvitation(int userId, int groupId, InvitationStatus status);
        bool Exists(int groupId);
    }
}
