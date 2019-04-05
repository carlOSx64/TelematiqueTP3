using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : Controller
    {
        private IGroupService groupService;
        private IUserService userService;

        public GroupsController(IGroupService groupService, IUserService userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> GetAll()
        {
            List<GroupDto> groups = this.groupService.GetAll().Select(g => this.ConvertGroupToGroupDto(g)).ToList();

            return Ok(groups);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{groupId}/users")]
        public ActionResult<string> GetUsersByGroupId(int groupId)
        {
            try
            {

                List<UserDto> groups = groupService.GetUsersByGroup(groupId).Select(g => this.ConvertUserToUserDto(g)).ToList();
                return Ok(groups);
            }
            catch
            {
                return BadRequest(new { message = "Could not get users for groupId: " + groupId });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Create([FromForm]Group group)
        {
            try
            {
                Group newGroup = this.groupService.Create(group.Name);
                return Ok(newGroup);
            }
            catch
            {
                return BadRequest(new { message = "Cannot create group" });
            }
        }

        [AllowAnonymous]
        [HttpPost("{groupId}/users/{userId}")]
        public ActionResult<string> AddUser(int groupId, int userId, [FromForm]bool isAdmin)
        {
            if (!this.groupService.Exists(groupId))
            {
                return BadRequest(new { message = "Invalid group id" });
            }

            if (!this.userService.Exists(userId))
            {
                return BadRequest(new { message = "Invalid user id" });
            }

            try
            {
                this.groupService.AddUserToGroup(userId, groupId, isAdmin);
                return Ok(new { message = "User added to group" });
            }
            catch
            {
                return BadRequest(new { message = "Could not add user " + userId + " to group " + groupId });
            }
        }

        [AllowAnonymous]
        [HttpDelete("{groupId}/users/{userId}")]
        public ActionResult<string> RemoveUser(int groupId, int userId)
        {
            if (!this.groupService.Exists(groupId))
            {
                return BadRequest(new { message = "Invalid group id" });
            }

            if (!this.userService.Exists(userId))
            {
                return BadRequest(new { message = "Invalid user id" });
            }

            try
            {
                this.groupService.RemoveUserFromGroup(userId, groupId);
                return Ok(new { message = "User removed from group" });
            }
            catch
            {
                return BadRequest(new { message = "Could not remove user " + userId + " from group " + groupId });
            }
        }

        [AllowAnonymous]
        [HttpPut("{groupId}/users/{userId}")]
        public ActionResult<string> EditUser(int groupId, int userId, [FromForm]bool isAdmin)
        {
            if (!this.groupService.Exists(groupId))
            {
                return BadRequest(new { message = "Invalid group id" });
            }

            if (!this.userService.Exists(userId))
            {
                return BadRequest(new { message = "Invalid user id" });
            }

            try
            {
                this.groupService.EditUserPermissions(userId, groupId, isAdmin);
                return Ok(new { message = "User permissions updated" });
            }
            catch
            {
                return BadRequest(new { message = "Could not edit user " + userId + " from group " + groupId });
            }
        }

        [AllowAnonymous]
        [HttpPost("{groupId}/invitations/{userId}")]
        public ActionResult<string> InviteUser(int groupId, int userId, [FromForm]bool isAdmin)
        {
            if (!this.groupService.Exists(groupId))
            {
                return BadRequest(new { message = "Invalid group id" });
            }

            if (!this.userService.Exists(userId))
            {
                return BadRequest(new { message = "Invalid user id" });
            }

            try
            {
                this.groupService.InviteUser(userId, groupId, isAdmin, 1); // TODO: Add current user id
                return Ok(new { message = "User invited" });
            }
            catch
            {
                return BadRequest(new { message = "Could not add user " + userId + " to group " + groupId });
            }
        }

        [AllowAnonymous]
        [HttpPut("{groupId}/invitations/{userId}")]
        public ActionResult<string> UpdateInvitation(int groupId, int userId, [FromForm] InvitationStatus status)
        {
            if (!this.groupService.Exists(groupId))
            {
                return BadRequest(new { message = "Invalid group id" });
            }

            if (!this.userService.Exists(userId))
            {
                return BadRequest(new { message = "Invalid user id" });
            }

            try
            {
                this.groupService.UpdateInvitation(userId, groupId, status);
                return Ok(new { message = "Invitation updated" });
            }
            catch
            {
                return BadRequest(new { message = "Could not add user " + userId + " to group " + groupId });
            }
        }

        private GroupDto ConvertGroupToGroupDto(Group group)
        {
            GroupDto groupDto = new GroupDto()
            {
                Id = group.Id,
                Name = group.Name,
                Members = group.UserGroups.Where(ug => !ug.IsAdmin).Select(ug => ug.UserId).ToList(),
                Administrators = group.UserGroups.Where(ug => ug.IsAdmin).Select(ug => ug.UserId).ToList()
            };

            return groupDto;
        }

        private UserDto ConvertUserToUserDto(User user)
        {
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username
            };

            return userDto;
        }
    }
}
