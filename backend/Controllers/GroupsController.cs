﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;
using WebApi.Helpers;

namespace WebApi.Controllers
{
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

        // GET api/groups
        // Anonymous
        [HttpGet]
        public ActionResult<string> GetAll()
        {
            List<GroupDto> groups = this.groupService.GetAll().Select(g => UserHelper.ConvertGroupToGroupDto(g)).ToList();

            return Ok(groups);
        }

        // GET api/groups/:id
        // Anonymous
        [HttpGet]
        [Route("{groupId}")]
        public ActionResult<string> Get(int groupId)
        {
            try
            {
                GroupDto group = UserHelper.ConvertGroupToGroupDto(this.groupService.Get(groupId));

                return Ok(group);
            }
            catch
            {
                return BadRequest(new { message = "Could not get group: " + groupId });
            }
        }


        // GET api/groups/:id/users
        // Anonymous
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


        // POST api/groups
        // Anonymous
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

        // POST api/groups/:groupId/users/:userId
        // Anonymous
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

        // DELETE api/groups/:groupId/users/:userId
        // Anonymous
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

        // PUT api/groups/:groupId/users/:userId
        // Anonymous
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

        // POST api/groups/:groupId/invitations/:userId
        // Anonymous
        [HttpPost("{groupId}/invitations/{userId}")]
        public ActionResult<string> InviteUser(int groupId, int userId, [FromForm]string apiKey, [FromForm]bool isAdmin)
        {
            if (!UserHelper.ValidateApiKey(apiKey, this.userService))
            {
                return BadRequest(new { message = "Invalid API key" });
            }

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
                var invitedById = UserHelper.GetUserByApiKey(apiKey, this.userService);
                this.groupService.InviteUser(userId, groupId, isAdmin, invitedById);
                return Ok(new { message = "User invited" });
            }
            catch
            {
                return BadRequest(new { message = "Could not add user " + userId + " to group " + groupId });
            }
        }


        // PUT api/groups/:groupId/invitations/:userId
        // Anonymous
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
                Invitation invitation = this.groupService.UpdateInvitation(userId, groupId, status);
                if (status == InvitationStatus.Accepted)
                {

                    this.groupService.AddUserToGroup(userId, groupId, invitation.IsAdmin);
                }
                return Ok(new { message = "Invitation updated" });
            }
            catch
            {
                return BadRequest(new { message = "Could not add user " + userId + " to group " + groupId });
            }
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
