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
            User user = _userService.authenticate(userParam.Username, userParam.Password);

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
            List<User> users = _userService.getAllUsers();

            return Ok(users);
        }
    }
}