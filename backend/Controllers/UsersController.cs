using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Data;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        private IUserService _userService;
        private IConnectedUserService _connectedUserService;

        public UsersController(IUserService userService, IConnectedUserService connectedUserService)
        {
            _userService = userService;
            _connectedUserService = connectedUserService;
        }

        // POST api/users/authenticate
        // Anonymous
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate([FromForm]User userParam)
        {
            User user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            // TODO : Validate it works
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            _connectedUserService.Connect(user.Id, ip);

            return Ok(new { id = user.Id, username = user.Username, apiKey = user.ApiKey});
        }

        // POST api/users/register
        // Anonymous
        [HttpPost("register")]
        public ActionResult<string> Register([FromForm]User userParam)
        {
            bool success = _userService.Create(userParam.Username, userParam.Password);

            if (!success)
                return BadRequest(new { message = "Failed to create new account" });

            return Ok();
        }

        // GET api/users/logout
        // Authenticated
        [HttpGet("logout")]
        public ActionResult<string> Logout([FromQuery]string apiKey)
        {
            if (!UserHelper.ValidateApiKey(apiKey, _userService)) {
                return BadRequest(new { message = "Invalid API key"});
            }

            var userId = UserHelper.GetUserByApiKey(apiKey, _userService);

            // Logout
            _connectedUserService.Logout(userId);

            return Ok();
        }

        // GET api/users
        // Anonymous
        [HttpGet]
        public ActionResult<string> GetAll()
        {
            List<UserDto> users = _userService.GetAll().Select(u => UserHelper.ConvertUserToUserDto(u)).ToList();

            return Ok(users);
        }

        // GET api/users/:id/groups
        // Anonymous
        [HttpGet]
        [Route("{userId}/groups")]
        public ActionResult<string> GetGroupsByUserId(int userId)
        {
            try
            {
                List<GroupDto> groups = _userService.GetGroupsByUser(userId).Select(g => UserHelper.ConvertGroupToGroupDto(g)).ToList();
                return Ok(groups);
            }
            catch
            {
                return BadRequest(new { message = "Could not get groups for userId: " + userId });
            }
        }

        // GET api/users/:id/invitations 
        // Anonymous
        [HttpGet]
        [Route("{userId}/invitations")]
        public ActionResult<string> GetInvitationsByUserId(int userId)
        {
            try
            {
                List<InvitationDto> invitations = _userService.GetInvitationsByUser(userId).Select(i => UserHelper.ConvertInvitationToInvitationDto(i)).ToList();
                return Ok(invitations);
            }
            catch
            {
                return BadRequest(new { message = "Could not get invitations for userId: " + userId });
            }
        }
    }
}