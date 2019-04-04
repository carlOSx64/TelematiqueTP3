using System;
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
        private IGroupService _groupService;

        public SetupController(IUserService userService, IGroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }


        // GET api/setup/reset
        [AllowAnonymous]
        [HttpGet("reset")]
        public ActionResult<string> reset()
        {
            Console.WriteLine("haha");
            // Truncate table users
            _userService.DeleteAll();
            _userService.Create("user1", "user1");
            _userService.Create("user2", "user2");
            _userService.Create("user3", "user3");
            _userService.Create("user4", "user4");
            _userService.Create("user5", "user5");

            _groupService.DeleteAll();
            _groupService.Create("IFT585");
            _groupService.Create("IFT606");
            return Ok();
        }
    }
}