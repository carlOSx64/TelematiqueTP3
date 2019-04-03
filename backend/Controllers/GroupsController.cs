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

        public GroupsController(IGroupService groupService)
        {
            this.groupService = groupService;
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
        public ActionResult<string> Create([FromForm]Group groupParam)
        {
            try
            {
                Group newGroup = this.groupService.Create(groupParam.Name);
                return Ok(newGroup);
            }
            catch
            {
                return BadRequest(new { message = "Cannot create group" });
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
