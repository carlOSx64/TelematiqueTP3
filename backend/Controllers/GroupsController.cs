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
            List<Group> groups = this.groupService.GetAll();

            return Ok(groups);
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
            catch (Exception e)
            {
                return BadRequest(new { message = "Cannot create group" });
            }
        }
    }
}
