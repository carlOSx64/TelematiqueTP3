using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WebApi.Models;
using WebApi.Data;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate([FromForm]User userParam)
        {
            User user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            return Ok(user);
        }

        // GET api/users
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> GetAll()
        {
            List<UserDto> users = _userService.GetAll().Select(u => this.ConvertUserToUserDto(u)).ToList();

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{userId}/groups")]
        public ActionResult<string> GetGroupsByUserId(int userId)
        {
            try
            {

                List<GroupDto> groups = _userService.GetGroupsByUser(userId).Select(g => this.ConverGroupToGroupDto(g)).ToList();
                return Ok(groups);
            }
            catch
            {
                return BadRequest(new { message = "Could not get groups for userId: " + userId });
            }
        }

        private UserDto ConvertUserToUserDto(User user)
        {
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Groups= user.UserGroups.Select(ug => ug.GroupId).ToList()
            };

            return userDto;
        }

        private GroupDto ConverGroupToGroupDto(Group group)
        {
            GroupDto groupDto = new GroupDto()
            {
                Id = group.Id,
                Name = group.Name
            };

            return groupDto;
        }
    }
}