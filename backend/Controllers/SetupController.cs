using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Data;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {

        private IUserService _userService;

        public SetupController(IUserService userService)
        {
            _userService = userService;
        }


        // GET api/setup/reset
        [AllowAnonymous]
        [HttpGet("reset")]
        public ActionResult<string> reset()
        {
            // Truncate table users
            _userService.deleteAll();   
            _userService.add("user1", "user1");
            _userService.add("user2", "user2");
            _userService.add("user3", "user3");
            _userService.add("user4", "user4");
            _userService.add("user5", "user5");
            return Ok();
        }
    }
}