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
    public class ConnectedUsersController : Controller
    {

        private IConnectedUserService _connectedUserService;

        public ConnectedUsersController(IConnectedUserService connectedUserService)
        {
            _connectedUserService = connectedUserService;
        }

        // GET api/connectedUsers
        // Anonymous
        [HttpGet]
        public ActionResult<string> GetAll()
        {
            List<ConnectedUser> connectedUsers = _connectedUserService.GetAll();

            return Ok(connectedUsers);
        }
    }
}