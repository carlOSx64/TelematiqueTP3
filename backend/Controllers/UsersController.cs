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
            // Users are hardcoded. We don't strip password / Api keys
            List<User> users = _userService.GetAll();

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{userId}/groups")]
        public ActionResult<string> GetGroupsByUserId(int userId)
        {
            try
            {

                List<Group> groups = _userService.GetGroupsByUser(userId);
                return Ok(groups);
            }
            catch
            {
                return BadRequest(new { message = "Could not get groups for userId: " + userId });
            }
        }
    }
}