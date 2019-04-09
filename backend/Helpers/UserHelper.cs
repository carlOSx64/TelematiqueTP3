using WebApi.Services;
using WebApi.Models;
using System.Linq;
using System;

namespace WebApi.Helpers
{
    public class UserHelper
    {
        // Only validates if the current API key is used by a user.
        public static int GetUserByApiKey(string apiKey, IUserService _userService)
        {
            if (!string.IsNullOrWhiteSpace(apiKey)) {
                Console.WriteLine(apiKey);
                User user = _userService.GetUserByApiKey(apiKey);

                if (user != null && !string.IsNullOrWhiteSpace(user.ApiKey) && !string.IsNullOrWhiteSpace(user.Username)) {
                    return user.Id;
                }
            }

            return -1;
        }

            // Syntaxic sugar for controller validation
        public static bool ValidateApiKey(string apiKey, IUserService _userService)
        {
            if (GetUserByApiKey(apiKey, _userService) == -1) {
                return false;
            }

            return true;
        }

        public static UserDto ConvertUserToUserDto(User user)
        {
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Groups = user.UserGroups.Select(ug => ug.GroupId).ToList()
            };

            return userDto;
        }

        public static GroupDto ConvertGroupToGroupDto(Group group)
        {
            GroupDto groupDto = new GroupDto()
            {
                Id = group.Id,
                Name = group.Name,
                Members = group.UserGroups.Where(ug => !ug.IsAdmin).Select(ug => ug.UserId).ToList(),
                Administrators = group.UserGroups.Where(ug => ug.IsAdmin).Select(ug => ug.UserId).ToList(),
                Files = group.Files.Where(f => f.GroupId == group.Id).Select(f => f.Id).ToList()
            };

            return groupDto;
        }
        public static InvitationDto ConvertInvitationToInvitationDto(Invitation invitation)
        {
            InvitationDto invitationDto = new InvitationDto()
            {
                UserId = invitation.UserId,
                GroupId = invitation.GroupId,
                IsAdmin = invitation.IsAdmin,
                Status = invitation.Status,
                InvitedById = invitation.InvitedById
            };

            return invitationDto;
        }
    }
}